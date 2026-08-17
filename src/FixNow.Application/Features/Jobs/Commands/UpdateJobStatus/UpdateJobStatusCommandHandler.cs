using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Jobs.Commands.UpdateJobStatus;

public sealed class UpdateJobStatusCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IJobRepository jobRepository,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateJobStatusCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        UpdateJobStatusCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate: a user without a
        //    technician profile cannot change a job status.
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

        // 3. Only the technician assigned to the job may change its status.
        //    An un-owned job is indistinguishable from a non-existent one,
        //    so job existence is never leaked.
        if (job.TechnicianProfileId != technicianProfile.Id)
        {
            return JobErrors.NotFound;
        }

        // 4. Apply the transition through the domain model. The aggregate
        //    owns the valid-transition rules; the client can never force an
        //    arbitrary status change.
        var transitionResult = job.ChangeStatus(command.Status);

        if (transitionResult.IsError)
        {
            return transitionResult.Errors;
        }

        // 5. Persist the change (committed by the transaction pipeline).
        //    Optimistic concurrency (PostgreSQL xmin row version) ensures a
        //    concurrent status change conflicts on the Job row.
        jobRepository.Update(job);

        return Result.Success;
    }
}
