namespace FixNow.Application.Features.Payments.Commands.HandlePaymentFailure;

public sealed record HandlePaymentFailureResponse(
    Guid PaymentId,
    PaymentStatus Status);
