namespace FixNow.Application.Features.Payments.Commands.InitiateOnlinePayment;

public sealed record OnlinePaymentResponse(
    Guid PaymentId,
    Guid JobId,
    Guid AssignmentId,
    PaymentMethod PaymentMethod,
    Money Amount,
    PaymentStatus Status,
    string? ProviderReference);
