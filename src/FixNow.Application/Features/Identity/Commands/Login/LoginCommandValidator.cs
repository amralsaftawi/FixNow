using FluentValidation;

namespace FixNow.Application.Features.Identity.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        ValidateLogin();

        ValidatePassword();
    }

    private void ValidateLogin()
    {
        RuleFor(x => x.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.Login.Required")
            .MaximumLength(256)
            .WithErrorCode("Identity.Login.TooLong");
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
}