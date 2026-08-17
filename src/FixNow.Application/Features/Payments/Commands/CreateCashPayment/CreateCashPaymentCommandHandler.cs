using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Helpers;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.Jobs.Queries.GetFinalJobPrice;

namespace FixNow.Application.Features.Payments.Commands.CreateCashPayment;

public sealed class CreateCashPaymentCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IJobRepository jobRepository,
    IAssignmentRepository assignmentRepository,
    IPaymentRepository paymentRepository,
    ICurrentUser currentUser)
    : ICommandHandler<CreateCashPaymentCommand, Result<CashPaymentResponse>>
{
    public async Task<Result<CashPaymentResponse>> Handle(
        CreateCashPaymentCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate: only a technician
        //    who collected cash can record the payment.
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. Verify the job exists and belongs to the authenticated
        //    technician. An un-owned job is indistinguishable from a
        //    non-existent one.
        var jobAccess = await jobRepository.GetAccessAsync(
            command.JobId,
            cancellationToken);

        if (jobAccess is null ||
            jobAccess.TechnicianProfileId != technicianProfile.Id)
        {
            return JobErrors.NotFound;
        }

        // 3. The accepted assignment must exist for this technician on the
        //    job's service request. The assignment anchors the Payment
        //    aggregate.
        var assignment = await assignmentRepository.GetAcceptedByRequestAndTechnicianAsync(
            jobAccess.ServiceRequestId,
            technicianProfile.Id,
            cancellationToken);

        if (assignment is null)
        {
            return JobErrors.NotFound;
        }

        // 4. A payment can only be recorded for a completed job. Only a
        //    completed job has an authoritative final price snapshot.
        var pricing = await jobRepository.GetFinalJobPriceAsync(
            command.JobId,
            cancellationToken);

        if (pricing is null)
        {
            return JobErrors.NotFound;
        }

        if (pricing.Status != JobStatus.Completed)
        {
            return PaymentErrors.JobNotCompleted;
        }

        // 5. Resolve the final price using the same authoritative
        //    computation that GET /api/jobs/{jobId}/price uses. This
        //    guarantees the payment amount always equals the displayed
        //    final price.
        var finalPrice = await FinalPriceResolver.ResolveAsync(
            pricing,
            jobAccess.TechnicianProfileId,
            technicianProfileRepository,
            cancellationToken);

        if (finalPrice is null)
        {
            return PaymentErrors.FinalPriceNotResolved;
        }

        // 6. Guard against duplicate payment for the same assignment.
        var alreadyExists = await paymentRepository.ExistsByAssignmentIdAsync(
            assignment.Id,
            cancellationToken);

        if (alreadyExists)
        {
            return PaymentErrors.AlreadyExists;
        }

        // 7. Create the Payment aggregate anchored to the job's accepted
        //    assignment. The amount is entirely server-derived from the
        //    final price computation above; the client supplies nothing
        //    beyond the job identifier.
        var paymentResult = Payment.Create(
            Guid.NewGuid(),
            assignment.Id,
            jobAccess.ServiceRequestCustomerProfileId,
            finalPrice,
            PaymentMethod.Cash);

        if (paymentResult.IsError)
        {
            return paymentResult.Errors;
        }

        // 8. For cash there is no external gateway: the technician has
        //    collected the money and the payment is immediately completed.
        var markPaidResult = paymentResult.Value.MarkAsPaid();

        if (markPaidResult.IsError)
        {
            return markPaidResult.Errors;
        }

        // 9. Persist the completed payment (committed atomically by the
        //    transaction pipeline).
        await paymentRepository.AddAsync(
            paymentResult.Value,
            cancellationToken);

        return paymentResult.Value.ToCashPaymentResponse(command.JobId);
    }
}
