using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Services;

namespace FixNow.Application.Features.Payments.Commands.RefundPayment;

public sealed class RefundPaymentCommandHandler(
    ICustomerRepository customerRepository,
    IPaymentRepository paymentRepository,
    IOnlinePaymentProvider onlinePaymentProvider,
    ICurrentUser currentUser)
    : ICommandHandler<RefundPaymentCommand, Result<RefundPaymentResponse>>
{
    public async Task<Result<RefundPaymentResponse>> Handle(
        RefundPaymentCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. Payment
        //    refund is a customer-only action: only the customer who
        //    owns the payment can request a refund.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Load the existing payment by id. If the payment does not
        //    exist or belongs to a different customer, the same NotFound
        //    error is returned to avoid leaking payment existence.
        var payment = await paymentRepository.GetByIdAsync(
            command.PaymentId,
            cancellationToken);

        if (payment is null ||
            payment.CustomerProfileId != customerProfile.Id)
        {
            return PaymentErrors.NotFound;
        }

        // 3. Only online-capable payment methods are eligible for
        //    provider refund. Cash payments are collected in person
        //    and have no external provider to refund through.
        if (payment.PaymentMethod != PaymentMethod.Card
            && payment.PaymentMethod != PaymentMethod.Wallet)
        {
            return PaymentErrors.PaymentNotProcessable;
        }

        // 4. If the payment is already Refunded, the operation is
        //    idempotent: return the current state without calling
        //    the provider again.
        if (payment.Status == PaymentStatus.Refunded)
        {
            return payment.ToRefundPaymentResponse();
        }

        // 5. Only a Paid payment can transition to Refunded. Pending
        //    and Failed payments have not completed their lifecycle.
        if (payment.Status != PaymentStatus.Paid)
        {
            return PaymentErrors.InvalidStatusTransition;
        }

        // 6. Determine the refund amount from the authoritative
        //    Payment.Amount. The client cannot supply an amount.
        var refundAmount = (decimal)payment.Amount;

        // 7. Delegate to the trusted provider refund boundary. The
        //    provider verifies the refund with the external gateway
        //    and, if successful, the domain state transition follows.
        //    When no real provider is configured, the stub returns
        //    an explicit error rather than faking success.
        //
        //    The payment ID serves as the idempotency key. If a real
        //    provider is configured later, duplicate calls with the
        //    same payment ID must not produce independent refunds.
        var refundResult = await onlinePaymentProvider.RefundAsync(
            payment.Id,
            refundAmount,
            payment.Amount.Currency.ToString(),
            cancellationToken);

        if (refundResult.IsError)
        {
            return refundResult.Errors;
        }

        // 8. Only when the provider abstraction reports trusted
        //    success: perform the domain state transition through
        //    Payment.MarkAsRefunded(). The method is idempotent:
        //    a second call returns AlreadyRefunded without side
        //    effects. PaymentRefundedDomainEvent is raised by the
        //    domain transition itself — not by this handler.
        var markRefundedResult = payment.MarkAsRefunded();

        if (markRefundedResult.IsError)
        {
            return markRefundedResult.Errors;
        }

        // 9. Persist the Refunded state. The change is committed
        //    atomically by the existing Unit of Work pipeline.
        return payment.ToRefundPaymentResponse();
    }
}
