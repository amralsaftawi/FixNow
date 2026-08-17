using FixNow.Application.Common.Interfaces.Services;

namespace FixNow.Infrastructure.Services;

public sealed class StubOnlinePaymentProvider : IOnlinePaymentProvider
{
    public Task<Result<OnlinePaymentInitiationResult>> InitiateAsync(
        Guid paymentId,
        decimal amount,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Result<OnlinePaymentInitiationResult>>(
            PaymentErrors.ProviderNotConfigured);
    }

    public Task<Result<OnlinePaymentProcessResult>> ProcessAsync(
        Guid paymentId,
        decimal amount,
        string currencyCode,
        string? providerReference,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Result<OnlinePaymentProcessResult>>(
            PaymentErrors.ProviderNotConfigured);
    }

    public Task<Result<OnlinePaymentRefundResult>> RefundAsync(
        Guid paymentId,
        decimal amount,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Result<OnlinePaymentRefundResult>>(
            PaymentErrors.ProviderNotConfigured);
    }
}
