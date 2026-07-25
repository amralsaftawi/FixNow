using FluentValidation;

namespace FixNow.Application.Features.ServiceCategories.Commands.UpdateServiceCategory;

public sealed class UpdateServiceCategoryCommandValidator
    : AbstractValidator<UpdateServiceCategoryCommand>
{
    public UpdateServiceCategoryCommandValidator()
    {
        ValidateServiceCategoryId();

        ValidateName();

        ValidateDescription();

        ValidateIconKey();

        ValidateDisplayOrder();
    }

    private void ValidateServiceCategoryId()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .WithErrorCode("ServiceCategory.Id.Required");
    }

    private void ValidateName()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("ServiceCategory.Name.Required")
            .MaximumLength(100)
            .WithErrorCode("ServiceCategory.Name.TooLong");
    }

    private void ValidateDescription()
    {
        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(500)
            .WithErrorCode("ServiceCategory.Description.TooLong");
    }

    private void ValidateIconKey()
    {
        RuleFor(x => x.IconKey)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("ServiceCategory.IconKey.Required");
    }

    private void ValidateDisplayOrder()
    {
        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("ServiceCategory.DisplayOrder.Invalid");
    }
}