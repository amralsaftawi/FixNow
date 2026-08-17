using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Jobs.Commands.MarkJobCompleted;

public sealed class MarkJobCompletedCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IJobRepository jobRepository,
    IAssignmentRepository assignmentRepository,
    ICurrentUser currentUser)
    : ICommandHandler<MarkJobCompletedCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        MarkJobCompletedCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate: a user without a
        //    technician profile cannot complete a job.
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

        // 3. Only the technician assigned to the job may complete it. An
        //    un-owned job is indistinguishable from a non-existent one, so
        //    job existence is never leaked.
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
        //    owns the valid-transition rules (InProgress -> Completed is the
        //    completion transition; Completed is a terminal state); the
        //    client can never force an arbitrary status change.
        var transitionResult = job.ChangeStatus(JobStatus.Completed);

        if (transitionResult.IsError)
        {
            return transitionResult.Errors;
        }

        // 6. Finalize the final price. Completion is the moment the price
        //    becomes authoritative: no further additional charges can be
        //    recorded, and the resolved service price and inspection fee are
        //    snapshotted onto the job so later technician or category pricing
        //    changes can never alter a completed job's price.
        var pricingSource = await jobRepository.GetPricingSourceAsync(
            job.Id,
            cancellationToken);

        if (pricingSource is null)
        {
            return JobErrors.NotFound;
        }

        var technicianPrice =
            await technicianProfileRepository.GetServicePriceByCategoryAsync(
                job.TechnicianProfileId,
                pricingSource.ServiceCategoryId,
                cancellationToken);

        var finalizeResult = job.FinalizePrice(
            technicianPrice ?? pricingSource.BasePrice,
            pricingSource.InspectionFee);

        if (finalizeResult.IsError)
        {
            return finalizeResult.Errors;
        }

        // 7. Persist the change (committed by the transaction pipeline).
        //    Optimistic concurrency (PostgreSQL xmin row version) ensures a
        //    concurrent status change conflicts on the Job row.
        jobRepository.Update(job);

        return Result.Success;
    }
}
