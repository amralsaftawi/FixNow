using FluentValidation;

namespace FixNow.Application.Features.GeographicData.Commands.CreateCity;

public sealed class CreateCityCommandValidator
    : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        ValidateCountryId();

        ValidateName();
    }

    private void ValidateCountryId()
    {
        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithErrorCode("City.CountryRequired");
    }

    private void ValidateName()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("City.NameRequired")
            .MaximumLength(100)
            .WithErrorCode("City.NameTooLong");
    }
}
