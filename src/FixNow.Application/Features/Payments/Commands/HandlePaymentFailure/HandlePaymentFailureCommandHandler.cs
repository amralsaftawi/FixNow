using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Services;

namespace FixNow.Application.Features.Payments.Commands.HandlePaymentFailure;

public sealed class HandlePaymentFailureCommandHandler(
    IPaymentRepository paymentRepository,
    IOnlinePaymentFailureService failureService)
    : ICommandHandler<HandlePaymentFailureCommand, Result<HandlePaymentFailureResponse>>
{
    public async Task<Result<HandlePaymentFailureResponse>> Handle(
        HandlePaymentFailureCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Load the payment by id. The payment must exist; otherwise
        //    the failure is rejected. This command is invoked by the
        //    trusted internal failure boundary, not by an arbitrary
        //    client, so no customer/technician authorization is applied.
        var payment = await paymentRepository.GetByIdAsync(
            command.PaymentId,
            cancellationToken);

        if (payment is null)
        {
            return PaymentErrors.NotFound;
        }

        // 2. Only online-capable payment methods are eligible for
        //    provider failure handling. Cash payments are collected in
        //    person and are already marked Paid at creation time.
        if (payment.PaymentMethod != PaymentMethod.Card
            && payment.PaymentMethod != PaymentMethod.Wallet)
        {
            return PaymentErrors.PaymentNotProcessable;
        }

        // 3. Only a Pending payment can transition to Failed. If the
        //    payment is already Failed, the operation is idempotent:
        //    MarkAsFailed returns AlreadyFailed without side effects.
        //    Paid/Refunded payments must not become Failed.
        if (payment.Status != PaymentStatus.Pending)
        {
            if (payment.Status == PaymentStatus.Failed)
            {
                return new HandlePaymentFailureResponse(
                    PaymentId: payment.Id,
                    Status: payment.Status);
            }

            return PaymentErrors.InvalidStatusTransition;
        }

        // 4. Delegate to the trusted failure service. The service
        //    invokes the provider verification boundary and, if the
        //    provider confirms the failure, performs the domain state
        //    transition through Payment.MarkAsFailed(). When no real
        //    provider is configured, the stub returns an explicit error
        //    rather than faking failure.
        //
        //    The payment ID and provider reference together form the
        //    idempotency identity. Duplicate failure callbacks for the
        //    same payment produce the same result without duplicate side
        //    effects: MarkAsFailed is already idempotent.
        var failureResult = await failureService.HandleFailureAsync(
            payment.Id,
            command.ProviderReference,
            cancellationToken);

        if (failureResult.IsError)
        {
            return failureResult.Errors;
        }

        return failureResult.Value.ToHandlePaymentFailureResponse();
    }
}
