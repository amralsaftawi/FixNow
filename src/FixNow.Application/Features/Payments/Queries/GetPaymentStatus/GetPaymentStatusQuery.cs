namespace FixNow.Application.Features.Payments.Queries.GetPaymentStatus;

public sealed record GetPaymentStatusQuery(
    Guid PaymentId)
    : IQuery<Result<GetPaymentStatusResponse>>;
