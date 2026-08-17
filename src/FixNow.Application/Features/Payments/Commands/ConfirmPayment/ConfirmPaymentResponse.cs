namespace FixNow.Application.Features.Payments.Commands.ConfirmPayment;

public sealed record ConfirmPaymentResponse(
    Guid PaymentId,
    PaymentStatus Status,
    DateTimeOffset PaidAt,
    string? ProviderReference);
