using FluentValidation;

namespace FixNow.Application.Features.Payments.Commands.InitiateOnlinePayment;

public sealed class InitiateOnlinePaymentCommandValidator
    : AbstractValidator<InitiateOnlinePaymentCommand>
{
    public InitiateOnlinePaymentCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");
    }
}
