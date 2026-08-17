namespace FixNow.Application.Common.Interfaces.Services;

public sealed record PaymentConfirmationResult(
    Guid PaymentId,
    PaymentStatus Status,
    DateTimeOffset PaidAt,
    string? ProviderReference);

public interface IOnlinePaymentConfirmationService
{
    Task<Result<PaymentConfirmationResult>> ConfirmAsync(
        Guid paymentId,
        string? providerReference,
        decimal confirmedAmount,
        string currencyCode,
        CancellationToken cancellationToken = default);
}
