namespace FixNow.Application.Features.Payments.Commands.CreateCashPayment;

public sealed record CashPaymentResponse(
    Guid PaymentId,
    Guid JobId,
    Guid AssignmentId,
    PaymentMethod PaymentMethod,
    Money Amount,
    PaymentStatus Status,
    DateTimeOffset PaidAt);
