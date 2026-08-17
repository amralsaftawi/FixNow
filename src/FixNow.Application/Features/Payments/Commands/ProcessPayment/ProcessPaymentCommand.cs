using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Payments.Commands.ProcessPayment;

public sealed record ProcessPaymentCommand(
    Guid PaymentId)
    : ICommand<Result<ProcessPaymentResponse>>;
