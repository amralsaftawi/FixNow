using FixNow.Application.Common.Interfaces.Services;

namespace FixNow.Infrastructure.Services;

public sealed class StubOnlinePaymentFailureService : IOnlinePaymentFailureService
{
    public Task<Result<PaymentFailureResult>> HandleFailureAsync(
        Guid paymentId,
        string? providerReference,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Result<PaymentFailureResult>>(
            PaymentErrors.ProviderNotConfigured);
    }
}
