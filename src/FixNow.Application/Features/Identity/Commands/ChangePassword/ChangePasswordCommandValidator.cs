using FluentValidation;

namespace FixNow.Application.Features.Identity.Commands.ChangePassword;

public sealed class ChangePasswordCommandValidator
    : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        ValidateCurrentPassword();

        ValidateNewPassword();

        ValidateConfirmPassword();
    }

    private void ValidateCurrentPassword()
    {
        RuleFor(x => x.CurrentPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.CurrentPassword.Required")
            .MaximumLength(100)
            .WithErrorCode("Identity.CurrentPassword.TooLong");
    }

    private void ValidateNewPassword()
    {
        RuleFor(x => x.NewPassword)
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
            .Equal(x => x.NewPassword)
            .WithErrorCode("Identity.Password.NotMatched");
    }
}
