using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Jobs.Commands.AddAdditionalServiceCharge;

public sealed class AddAdditionalServiceChargeCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IJobRepository jobRepository,
    IAssignmentRepository assignmentRepository,
    ICurrentUser currentUser)
    : ICommandHandler<AddAdditionalServiceChargeCommand, Result<AdditionalServiceChargeResponse>>
{
    public async Task<Result<AdditionalServiceChargeResponse>> Handle(
        AddAdditionalServiceChargeCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate: a user without a
        //    technician profile cannot add additional charges. The
        //    technician is always derived from the authenticated identity,
        //    never from client input.
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

        // 3. Only the technician assigned to the job may add charges to it.
        //    An un-owned job is indistinguishable from a non-existent one, so
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

        // 5. Build a valid amount using the domain monetary rules. The
        //    charge is a separate pricing component: the base service price,
        //    inspection fee, and technician service price are never modified.
        var amountResult = Money.Create(
            command.Amount,
            command.Currency);

        if (amountResult.IsError)
        {
            return amountResult.Errors;
        }

        // 6. Create the charge through the domain model.
        var chargeResult = JobAdditionalCharge.Create(
            Guid.NewGuid(),
            job.Id,
            command.Description,
            amountResult.Value);

        if (chargeResult.IsError)
        {
            return chargeResult.Errors;
        }

        // 7. Add the charge to the job. The aggregate enforces the lifecycle
        //    rule: charges cannot be recorded against a completed or
        //    cancelled (terminated) job.
        var addResult = job.AddAdditionalCharge(chargeResult.Value);

        if (addResult.IsError)
        {
            return addResult.Errors;
        }

        // 8. Persist the charge (committed by the transaction pipeline). The
        //    charge row references the job; no existing pricing is touched.
        jobRepository.Update(job);

        return chargeResult.Value.ToAdditionalServiceChargeResponse();
    }
}
