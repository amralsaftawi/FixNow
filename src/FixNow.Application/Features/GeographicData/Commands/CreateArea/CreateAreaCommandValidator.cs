using FluentValidation;

namespace FixNow.Application.Features.GeographicData.Commands.CreateArea;

public sealed class CreateAreaCommandValidator
    : AbstractValidator<CreateAreaCommand>
{
    public CreateAreaCommandValidator()
    {
        ValidateCityId();

        ValidateName();
    }

    private void ValidateCityId()
    {
        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .WithErrorCode("Area.CityRequired");
    }

    private void ValidateName()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Area.NameRequired")
            .MaximumLength(100)
            .WithErrorCode("Area.NameTooLong");
    }
}
