using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Jobs.Queries.GetJobTimeline;

public sealed class GetJobTimelineQueryHandler(
    ICustomerRepository customerRepository,
    ITechnicianProfileRepository technicianProfileRepository,
    IJobRepository jobRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetJobTimelineQuery, Result<GetJobTimelineResponse>>
{
    public async Task<Result<GetJobTimelineResponse>> Handle(
        GetJobTimelineQuery query,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's profiles. The timeline is
        //    accessible to the customer who owns the job's service request
        //    and to the technician the job is assigned to. A user holding
        //    neither profile can never be authorized for a job.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        // 2. Load the job's ownership facts as a lightweight projection.
        //    The full Job aggregate (and its ServiceRequest graph) is never
        //    loaded for authorization.
        var jobAccess = await jobRepository.GetAccessAsync(
            query.JobId,
            cancellationToken);

        if (jobAccess is null)
        {
            return JobErrors.NotFound;
        }

        // 3. Authorize. Customer access is derived from the authenticated
        //    identity through CustomerProfile -> ServiceRequest -> Job, and
        //    technician access from the technician assigned to the job. An
        //    out-of-scope job is indistinguishable from a non-existent one,
        //    so job existence is never leaked through JobId manipulation.
        var isCustomerOwner =
            customerProfile is not null
            && customerProfile.Id == jobAccess.ServiceRequestCustomerProfileId;

        var isAssignedTechnician =
            technicianProfile is not null
            && technicianProfile.Id == jobAccess.TechnicianProfileId;

        if (!isCustomerOwner && !isAssignedTechnician)
        {
            return JobErrors.NotFound;
        }

        // 4. Query the persisted timeline with database-side pagination,
        //    projection, and deterministic chronological ordering.
        var result = await jobRepository.GetTimelineAsync(
            query.JobId,
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        return new GetJobTimelineResponse(
            Items: result.Items,
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}
