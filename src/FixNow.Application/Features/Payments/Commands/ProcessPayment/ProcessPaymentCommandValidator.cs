using FluentValidation;

namespace FixNow.Application.Features.Payments.Commands.ProcessPayment;

public sealed class ProcessPaymentCommandValidator
    : AbstractValidator<ProcessPaymentCommand>
{
    public ProcessPaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithErrorCode("Payment.IdRequired");
    }
}
