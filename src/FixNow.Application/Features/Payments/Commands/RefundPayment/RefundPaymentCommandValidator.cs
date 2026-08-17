using FluentValidation;

namespace FixNow.Application.Features.Payments.Commands.RefundPayment;

public sealed class RefundPaymentCommandValidator
    : AbstractValidator<RefundPaymentCommand>
{
    public RefundPaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithErrorCode("Payment.IdRequired");
    }
}
