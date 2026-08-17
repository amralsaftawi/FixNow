namespace FixNow.Application.Features.Payments.Commands.RefundPayment;

public sealed record RefundPaymentResponse(
    Guid PaymentId,
    PaymentStatus Status);
