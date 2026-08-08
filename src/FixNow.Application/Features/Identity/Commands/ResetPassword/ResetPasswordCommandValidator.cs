using FluentValidation;

namespace FixNow.Application.Features.Identity.Commands.ResetPassword;

public sealed class ResetPasswordCommandValidator
    : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty()
            .WithMessage("Identifier is required.")
            .MaximumLength(254)
            .WithMessage("Identifier must not exceed 254 characters.");

        RuleFor(x => x.Otp)
            .NotEmpty()
            .WithMessage("OTP is required.")
            .Length(6)
            .WithMessage("OTP must be exactly 6 digits.")
            .Matches(@"^\d{6}$")
            .WithMessage("OTP must contain only digits.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.")
            .MinimumLength(8)
            .WithMessage("New password must be at least 8 characters long.")
            .MaximumLength(128)
            .WithMessage("New password must not exceed 128 characters.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Password confirmation is required.")
            .Equal(x => x.NewPassword)
            .WithMessage("Password confirmation does not match the new password.");
    }
}