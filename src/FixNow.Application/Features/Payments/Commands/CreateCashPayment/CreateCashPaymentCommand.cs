using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Payments.Commands.CreateCashPayment;

public sealed record CreateCashPaymentCommand(
    Guid JobId)
    : ICommand<Result<CashPaymentResponse>>;
