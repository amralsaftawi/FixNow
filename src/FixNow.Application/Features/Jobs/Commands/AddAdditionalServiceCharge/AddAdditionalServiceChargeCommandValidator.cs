using FluentValidation;

namespace FixNow.Application.Features.Jobs.Commands.AddAdditionalServiceCharge;

public sealed class AddAdditionalServiceChargeCommandValidator
    : AbstractValidator<AddAdditionalServiceChargeCommand>
{
    public AddAdditionalServiceChargeCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithErrorCode("JobAdditionalCharge.DescriptionRequired")
            .MaximumLength(500)
            .WithErrorCode("JobAdditionalCharge.DescriptionTooLong");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithErrorCode("Money.Amount.Invalid");

        RuleFor(x => x.Currency)
            .IsInEnum()
            .WithErrorCode("Money.Currency.Invalid");
    }
}
