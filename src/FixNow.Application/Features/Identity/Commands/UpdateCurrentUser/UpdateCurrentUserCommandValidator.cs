using FluentValidation;

namespace FixNow.Application.Features.Identity.Commands.UpdateCurrentUser;

public sealed class UpdateCurrentUserCommandValidator
    : AbstractValidator<UpdateCurrentUserCommand>
{
    public UpdateCurrentUserCommandValidator()
    {
        ValidateFirstName();

        ValidateLastName();

        ValidateEmail();

        ValidatePhoneNumber();

        ValidateCountryCode();

        ValidatePreferredLanguage();
    }

    private void ValidateFirstName()
    {
        RuleFor(x => x.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.FirstName.Required")
            .MaximumLength(100)
            .WithErrorCode("Identity.FirstName.TooLong");
    }

    private void ValidateLastName()
    {
        RuleFor(x => x.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.LastName.Required")
            .MaximumLength(100)
            .WithErrorCode("Identity.LastName.TooLong");
    }

    private void ValidateEmail()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithErrorCode("Identity.Email.Invalid")
            .MaximumLength(320)
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithErrorCode("Identity.Email.TooLong");
    }

    private void ValidatePhoneNumber()
    {
        RuleFor(x => x.PhoneNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.PhoneNumber.Required")
            .MaximumLength(20)
            .WithErrorCode("Identity.PhoneNumber.TooLong");
    }

    private void ValidateCountryCode()
    {
        RuleFor(x => x.CountryCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.CountryCode.Required")
            .Length(2)
            .WithErrorCode("Identity.CountryCode.Invalid");
    }

    private void ValidatePreferredLanguage()
    {
        RuleFor(x => x.PreferredLanguage)
            .IsInEnum()
            .WithErrorCode("Identity.PreferredLanguage.Invalid");
    }
}
