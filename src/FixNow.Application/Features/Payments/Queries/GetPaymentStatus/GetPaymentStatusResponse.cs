namespace FixNow.Application.Features.Payments.Queries.GetPaymentStatus;

public sealed record GetPaymentStatusResponse(
    Guid PaymentId,
    PaymentStatus Status,
    PaymentMethod PaymentMethod,
    Money Amount,
    DateTimeOffset? PaidAt);
