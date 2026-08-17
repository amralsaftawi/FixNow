using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Payments.Commands.RefundPayment;

public sealed record RefundPaymentCommand(
    Guid PaymentId)
    : ICommand<Result<RefundPaymentResponse>>;
