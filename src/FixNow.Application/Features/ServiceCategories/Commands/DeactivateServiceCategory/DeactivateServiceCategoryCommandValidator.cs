using FluentValidation;

namespace FixNow.Application.Features.ServiceCategories.Commands.DeactivateServiceCategory;

public sealed class DeactivateServiceCategoryCommandValidator
    : AbstractValidator<DeactivateServiceCategoryCommand>
{
    public DeactivateServiceCategoryCommandValidator()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .WithErrorCode("ServiceCategory.Id.Required");
    }
}