using FluentValidation;

namespace FixNow.Application.Features.Identity.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        ValidateFirstName();

        ValidateLastName();

        ValidateEmail();

        ValidatePhoneNumber();

        ValidatePassword();

        ValidateConfirmPassword();
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
            .NotEmpty()
            .WithErrorCode("Identity.Email.Required")
            .EmailAddress()
            .WithErrorCode("Identity.Email.Invalid")
            .MaximumLength(256)
            .WithErrorCode("Identity.Email.TooLong");
    }

    private void ValidatePhoneNumber()
    {
        RuleFor(x => x.PhoneNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.Phone.Required")
            .Matches(@"^\+?[1-9]\d{7,14}$")
            .WithErrorCode("Identity.Phone.Invalid");
    }

    private void ValidatePassword()
    {
        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.Password.Required")
            .MinimumLength(8)
            .WithErrorCode("Identity.Password.TooShort")
            .MaximumLength(100)
            .WithErrorCode("Identity.Password.TooLong");
    }

    private void ValidateConfirmPassword()
    {
        RuleFor(x => x.ConfirmPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.ConfirmPassword.Required")
            .Equal(x => x.Password)
            .WithErrorCode("Identity.Password.NotMatched");
    }
}