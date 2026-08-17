using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Services;

namespace FixNow.Application.Features.Payments.Commands.ProcessPayment;

public sealed class ProcessPaymentCommandHandler(
    ICustomerRepository customerRepository,
    IPaymentRepository paymentRepository,
    IOnlinePaymentProvider onlinePaymentProvider,
    ICurrentUser currentUser)
    : ICommandHandler<ProcessPaymentCommand, Result<ProcessPaymentResponse>>
{
    public async Task<Result<ProcessPaymentResponse>> Handle(
        ProcessPaymentCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. Payment
        //    processing is a customer-only action: only the customer who
        //    owns the payment can trigger processing.
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

        // 3. Only online-capable payment methods can enter the processing
        //    pipeline. Cash payments are collected in person and are
        //    already marked Paid at creation time.
        if (payment.PaymentMethod != PaymentMethod.Card
            && payment.PaymentMethod != PaymentMethod.Wallet)
        {
            return PaymentErrors.PaymentNotProcessable;
        }

        // 4. Only a Pending payment can be processed. Paid, Failed, and
        //    Refunded payments have already completed their lifecycle.
        if (payment.Status != PaymentStatus.Pending)
        {
            return PaymentErrors.PaymentNotProcessable;
        }

        // 5. Invoke the configured payment provider. The provider is
        //    resolved server-side from the payment method; the client
        //    cannot select an arbitrary provider. When no real provider
        //    is configured, the stub returns an explicit error rather
        //    than faking success.
        //
        //    The payment ID serves as the idempotency key. If a real
        //    provider is configured later, duplicate calls with the same
        //    payment ID must not produce independent transactions.
        var processResult = await onlinePaymentProvider.ProcessAsync(
            payment.Id,
            payment.Amount,
            payment.Amount.Currency.ToString(),
            payment.ProviderReference,
            cancellationToken);

        if (processResult.IsError)
        {
            return processResult.Errors;
        }

        // 6. Persist any provider reference returned by the processing
        //    step. The payment status is NOT changed here: that happens
        //    only during Payment Confirmation, which is a separate
        //    roadmap feature.
        payment.SetProviderReference(
            processResult.Value.ProviderReference);

        return payment.ToProcessPaymentResponse();
    }
}
