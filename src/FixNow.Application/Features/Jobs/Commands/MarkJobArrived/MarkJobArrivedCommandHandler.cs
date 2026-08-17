using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Jobs.Commands.MarkJobArrived;

public sealed class MarkJobArrivedCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IJobRepository jobRepository,
    IAssignmentRepository assignmentRepository,
    ICurrentUser currentUser)
    : ICommandHandler<MarkJobArrivedCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        MarkJobArrivedCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate: a user without a
        //    technician profile cannot mark a job as arrived.
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. Load the job.
        var job = await jobRepository.GetByIdAsync(
            command.JobId,
            cancellationToken);

        if (job is null)
        {
            return JobErrors.NotFound;
        }

        // 3. Only the technician assigned to the job may mark it as arrived.
        //    An un-owned job is indistinguishable from a non-existent one,
        //    so job existence is never leaked.
        if (job.TechnicianProfileId != technicianProfile.Id)
        {
            return JobErrors.NotFound;
        }

        // 4. The assignment must still be active - accepted, not rejected,
        //    cancelled, or otherwise inactive - for the technician on the
        //    job's service request. An inactive assignment grants no access,
        //    and the outcome is indistinguishable from a non-existent job.
        var assignment = await assignmentRepository.GetAcceptedByRequestAndTechnicianAsync(
            job.ServiceRequestId,
            technicianProfile.Id,
            cancellationToken);

        if (assignment is null)
        {
            return JobErrors.NotFound;
        }

        // 5. Apply the transition through the domain model. The aggregate
        //    owns the valid-transition rules (OnTheWay -> Arrived is the
        //    arrival transition); the client can never force an arbitrary
        //    status change.
        var transitionResult = job.ChangeStatus(JobStatus.Arrived);

        if (transitionResult.IsError)
        {
            return transitionResult.Errors;
        }

        // 6. Persist the change (committed by the transaction pipeline).
        //    Optimistic concurrency (PostgreSQL xmin row version) ensures a
        //    concurrent status change conflicts on the Job row.
        jobRepository.Update(job);

        return Result.Success;
    }
}
