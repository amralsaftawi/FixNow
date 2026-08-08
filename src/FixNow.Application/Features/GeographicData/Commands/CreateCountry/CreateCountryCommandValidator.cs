using FluentValidation;

namespace FixNow.Application.Features.GeographicData.Commands.CreateCountry;

public sealed class CreateCountryCommandValidator
    : AbstractValidator<CreateCountryCommand>
{
    public CreateCountryCommandValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Country.NameRequired")
            .MaximumLength(100)
            .WithErrorCode("Country.NameTooLong");
    }
}
