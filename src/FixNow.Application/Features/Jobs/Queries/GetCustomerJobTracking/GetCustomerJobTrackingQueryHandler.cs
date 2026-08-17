using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Jobs.Queries.GetCustomerJobTracking;

public sealed class GetCustomerJobTrackingQueryHandler(
    ICustomerRepository customerRepository,
    IServiceRequestRepository serviceRequestRepository,
    IJobRepository jobRepository,
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetCustomerJobTrackingQuery, Result<GetCustomerJobTrackingResponse>>
{
    public async Task<Result<GetCustomerJobTrackingResponse>> Handle(
        GetCustomerJobTrackingQuery query,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. This is
        //    also the customer-only authorization gate: a user without a
        //    customer profile cannot track jobs.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Load the job.
        var job = await jobRepository.GetByIdAsync(
            query.JobId,
            cancellationToken);

        if (job is null)
        {
            return JobErrors.NotFound;
        }

        // 3. Load the job's service request and verify it belongs to the
        //    authenticated customer. Ownership is derived from the
        //    authenticated identity through CustomerProfile -> ServiceRequest
        //    -> Job; an un-owned job is indistinguishable from a non-existent
        //    one, so job existence is never leaked.
        var serviceRequest = await serviceRequestRepository.GetByIdAsync(
            job.ServiceRequestId,
            cancellationToken);

        if (serviceRequest is null ||
            serviceRequest.CustomerProfileId != customerProfile.Id)
        {
            return JobErrors.NotFound;
        }

        // 4. The customer can always observe the final Job lifecycle state,
        //    but technician location access exists only because of an active
        //    Job relationship. A terminal job (completed or cancelled) no
        //    longer exposes the technician's location.
        if (job.IsTerminated)
        {
            return new GetCustomerJobTrackingResponse(
                JobId: job.Id,
                Status: job.Status,
                Latitude: null,
                Longitude: null);
        }

        // 5. Read the latest technician location produced by the existing
        //    Real-Time Technician Location mechanism (single record, updated
        //    in place on the technician profile). No location history is
        //    exposed, and a location that has never been published simply
        //    resolves to null.
        var technicianProfile = await technicianProfileRepository.GetByIdAsync(
            job.TechnicianProfileId,
            cancellationToken);

        return new GetCustomerJobTrackingResponse(
            JobId: job.Id,
            Status: job.Status,
            Latitude: technicianProfile?.Latitude,
            Longitude: technicianProfile?.Longitude);
    }
}
