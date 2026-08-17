using FluentValidation;

namespace FixNow.Application.Features.Payments.Commands.CreateCashPayment;

public sealed class CreateCashPaymentCommandValidator
    : AbstractValidator<CreateCashPaymentCommand>
{
    public CreateCashPaymentCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }
}
