using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Services;

namespace FixNow.Application.Features.Payments.Commands.ConfirmPayment;

public sealed class ConfirmPaymentCommandHandler(
    IPaymentRepository paymentRepository,
    IOnlinePaymentConfirmationService confirmationService)
    : ICommandHandler<ConfirmPaymentCommand, Result<ConfirmPaymentResponse>>
{
    public async Task<Result<ConfirmPaymentResponse>> Handle(
        ConfirmPaymentCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Load the payment by id. The payment must exist; otherwise
        //    the confirmation is rejected. This command is invoked by
        //    the trusted internal confirmation boundary, not by an
        //    arbitrary client, so no customer authorization is applied.
        var payment = await paymentRepository.GetByIdAsync(
            command.PaymentId,
            cancellationToken);

        if (payment is null)
        {
            return PaymentErrors.NotFound;
        }

        // 2. Only online-capable payment methods are eligible for
        //    provider confirmation. Cash payments are collected in
        //    person and are already marked Paid at creation time.
        if (payment.PaymentMethod != PaymentMethod.Card
            && payment.PaymentMethod != PaymentMethod.Wallet)
        {
            return PaymentErrors.PaymentNotProcessable;
        }

        // 3. Only a Pending payment can transition to Paid. If the
        //    payment is already Paid, the operation is idempotent:
        //    MarkAsPaid returns AlreadyPaid without side effects.
        if (payment.Status != PaymentStatus.Pending)
        {
            if (payment.Status == PaymentStatus.Paid)
            {
                return new ConfirmPaymentResponse(
                    PaymentId: payment.Id,
                    Status: payment.Status,
                    PaidAt: payment.PaidAt!.Value,
                    ProviderReference: payment.ProviderReference);
            }

            return PaymentErrors.InvalidStatusTransition;
        }

        // 4. Verify the provider-supplied amount matches the persisted
        //    payment amount. A mismatch indicates a provider-side
        //    discrepancy that must not silently pass.
        if (command.ConfirmedAmount != (decimal)payment.Amount)
        {
            return PaymentErrors.AmountMismatch;
        }

        // 5. Delegate to the trusted confirmation service. The service
        //    invokes the provider verification boundary and, if the
        //    provider confirms the transaction, performs the domain
        //    state transition through Payment.MarkAsPaid(). When no
        //    real provider is configured, the stub returns an explicit
        //    error rather than faking confirmation.
        //
        //    The payment ID and provider reference together form the
        //    idempotency identity. Duplicate confirmations for the same
        //    payment produce the same result without duplicate side
        //    effects: MarkAsPaid is already idempotent.
        var confirmationResult = await confirmationService.ConfirmAsync(
            payment.Id,
            command.ProviderReference,
            command.ConfirmedAmount,
            command.CurrencyCode,
            cancellationToken);

        if (confirmationResult.IsError)
        {
            return confirmationResult.Errors;
        }

        return confirmationResult.Value.ToConfirmPaymentResponse();
    }
}
