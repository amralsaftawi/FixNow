using FluentValidation;

namespace FixNow.Application.Features.ServiceCategories.Commands.ConfigureServiceCategoryInspectionFee;

public sealed class ConfigureServiceCategoryInspectionFeeCommandValidator
    : AbstractValidator<ConfigureServiceCategoryInspectionFeeCommand>
{
    public ConfigureServiceCategoryInspectionFeeCommandValidator()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .WithErrorCode("ServiceCategory.Id.Required");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithErrorCode("Money.Amount.Invalid");

        RuleFor(x => x.Currency)
            .IsInEnum()
            .WithErrorCode("Money.Currency.Invalid");
    }
}
