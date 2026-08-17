using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianRequests.Commands.ConvertServiceRequestToJob;

public sealed class ConvertServiceRequestToJobCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IServiceRequestRepository serviceRequestRepository,
    IAssignmentRepository assignmentRepository,
    IJobRepository jobRepository,
    ICurrentUser currentUser)
    : ICommandHandler<ConvertServiceRequestToJobCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        ConvertServiceRequestToJobCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate: a user without a
        //    technician profile cannot convert service requests to jobs.
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. Load the service request.
        var serviceRequest = await serviceRequestRepository.GetByIdAsync(
            command.ServiceRequestId,
            cancellationToken);

        if (serviceRequest is null)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 3. Only the technician holding the accepted assignment for the
        //    request may convert it. A rejected, cancelled, or otherwise
        //    inactive assignment grants no access, and an out-of-scope
        //    request is indistinguishable from a non-existent one, so
        //    request existence is never leaked.
        var assignment = await assignmentRepository.GetAcceptedByRequestAndTechnicianAsync(
            serviceRequest.Id,
            technicianProfile.Id,
            cancellationToken);

        if (assignment is null)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 4. A terminal request can never become a job.
        if (serviceRequest.Status == ServiceRequestStatus.Cancelled)
        {
            return JobErrors.RequestCancelled;
        }

        if (serviceRequest.Status == ServiceRequestStatus.Completed)
        {
            return JobErrors.RequestCompleted;
        }

        // 5. A request can be converted only once. The unique index on
        //    Jobs.ServiceRequestId is the database-level backstop for
        //    concurrent conversion attempts.
        var existingJob = await jobRepository.GetByServiceRequestIdAsync(
            serviceRequest.Id,
            cancellationToken);

        if (existingJob is not null)
        {
            return JobErrors.AlreadyConverted;
        }

        // 6. Create the job through the domain model. The job is established
        //    against the service request and its assigned technician; the
        //    ServiceRequest status itself is intentionally left unchanged.
        var jobResult = Job.Create(
            Guid.NewGuid(),
            serviceRequest.Id,
            technicianProfile.Id);

        if (jobResult.IsError)
        {
            return jobResult.Errors;
        }

        // 7. Record the conversion on the request timeline using the
        //    existing timeline mechanism.
        var recordResult = serviceRequest.RecordJobConversion();

        if (recordResult.IsError)
        {
            return recordResult.Errors;
        }

        // 8. Persist both changes (committed by the transaction pipeline).
        //    Optimistic concurrency (PostgreSQL xmin row version) ensures a
        //    concurrent conversion conflicts on the ServiceRequest row.
        serviceRequestRepository.Update(serviceRequest);

        await jobRepository.AddAsync(
            jobResult.Value,
            cancellationToken);

        return Result.Success;
    }
}
