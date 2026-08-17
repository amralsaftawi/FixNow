using FixNow.Application.Common.Interfaces.Services;

namespace FixNow.Infrastructure.Services;

public sealed class StubOnlinePaymentConfirmationService : IOnlinePaymentConfirmationService
{
    public Task<Result<PaymentConfirmationResult>> ConfirmAsync(
        Guid paymentId,
        string? providerReference,
        decimal confirmedAmount,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Result<PaymentConfirmationResult>>(
            PaymentErrors.ProviderNotConfigured);
    }
}
