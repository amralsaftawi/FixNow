namespace FixNow.Application.Common.Interfaces.Services;

public sealed record PaymentFailureResult(
    Guid PaymentId,
    PaymentStatus Status);

public interface IOnlinePaymentFailureService
{
    Task<Result<PaymentFailureResult>> HandleFailureAsync(
        Guid paymentId,
        string? providerReference,
        CancellationToken cancellationToken = default);
}
