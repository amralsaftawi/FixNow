using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Payments.Commands.HandlePaymentFailure;

public sealed record HandlePaymentFailureCommand(
    Guid PaymentId,
    string? ProviderReference)
    : ICommand<Result<HandlePaymentFailureResponse>>;
