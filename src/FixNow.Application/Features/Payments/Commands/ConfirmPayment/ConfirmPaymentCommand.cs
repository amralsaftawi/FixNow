using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Payments.Commands.ConfirmPayment;

public sealed record ConfirmPaymentCommand(
    Guid PaymentId,
    string? ProviderReference,
    decimal ConfirmedAmount,
    string CurrencyCode)
    : ICommand<Result<ConfirmPaymentResponse>>;
