namespace FixNow.Application.Features.Payments.Commands.ProcessPayment;

public sealed record ProcessPaymentResponse(
    Guid PaymentId,
    PaymentStatus Status,
    string? ProviderReference);
