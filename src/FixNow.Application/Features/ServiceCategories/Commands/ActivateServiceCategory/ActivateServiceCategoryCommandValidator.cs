using FluentValidation;

namespace FixNow.Application.Features.ServiceCategories.Commands.ActivateServiceCategory;

public sealed class ActivateServiceCategoryCommandValidator
    : AbstractValidator<ActivateServiceCategoryCommand>
{
    public ActivateServiceCategoryCommandValidator()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .WithErrorCode("ServiceCategory.Id.Required");
    }
}