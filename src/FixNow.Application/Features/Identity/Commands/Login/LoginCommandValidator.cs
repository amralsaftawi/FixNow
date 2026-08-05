using FluentValidation;

namespace FixNow.Application.Features.Identity.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        ValidateIdentifier();

        ValidatePassword();
    }

    private void ValidateIdentifier()
    {
        RuleFor(x => x.Identifier)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.Identifier.Required")
            .MaximumLength(256)
            .WithErrorCode("Identity.Identifier.TooLong");
    }

    private void ValidatePassword()
    {
        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.Password.Required")
            .MaximumLength(100)
            .WithErrorCode("Identity.Password.TooLong");
    }
}