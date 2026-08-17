namespace FixNow.Application.Common.Interfaces.Services;

public sealed record OnlinePaymentInitiationResult(
    string? ProviderReference);

public sealed record OnlinePaymentInitiationError(
    string Code,
    string Description);

public sealed record OnlinePaymentProcessResult(
    string? ProviderReference);

public sealed record OnlinePaymentRefundResult(
    Guid PaymentId);

public interface IOnlinePaymentProvider
{
    Task<Result<OnlinePaymentInitiationResult>> InitiateAsync(
        Guid paymentId,
        decimal amount,
        string currencyCode,
        CancellationToken cancellationToken = default);

    Task<Result<OnlinePaymentProcessResult>> ProcessAsync(
        Guid paymentId,
        decimal amount,
        string currencyCode,
        string? providerReference,
        CancellationToken cancellationToken = default);

    Task<Result<OnlinePaymentRefundResult>> RefundAsync(
        Guid paymentId,
        decimal amount,
        string currencyCode,
        CancellationToken cancellationToken = default);
}
