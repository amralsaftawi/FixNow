using FluentValidation;

namespace FixNow.Application.Features.ServiceCategories.Commands.CreateServiceCategory;

public sealed class CreateServiceCategoryCommandValidator
    : AbstractValidator<CreateServiceCategoryCommand>
{
    public CreateServiceCategoryCommandValidator()
    {
        ValidateName();

        ValidateDescription();

        ValidateIconKey();
        
        ValidateDisplayOrder();
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
            .NotEmpty()
            .WithErrorCode("ServiceCategory.Description.Required")
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