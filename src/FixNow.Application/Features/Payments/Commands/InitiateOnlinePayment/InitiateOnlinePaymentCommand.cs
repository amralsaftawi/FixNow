using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Payments.Commands.InitiateOnlinePayment;

public sealed record InitiateOnlinePaymentCommand(
    Guid JobId)
    : ICommand<Result<OnlinePaymentResponse>>;
