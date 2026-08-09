using FluentValidation;

namespace FixNow.Application.Features.ServiceCategories.Commands.RemoveServiceCategoryIcon;

public sealed class RemoveServiceCategoryIconCommandValidator
    : AbstractValidator<RemoveServiceCategoryIconCommand>
{
    public RemoveServiceCategoryIconCommandValidator()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty().WithErrorCode("ServiceCategory.Id.Required");
    }
}
