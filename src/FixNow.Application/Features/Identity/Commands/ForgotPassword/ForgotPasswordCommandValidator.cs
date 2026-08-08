using FluentValidation;

namespace FixNow.Application.Features.Identity.Commands.ForgotPassword;

public sealed class ForgotPasswordCommandValidator
    : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty()
            .WithMessage("Identifier is required.")
            .MaximumLength(254)
            .WithMessage("Identifier must not exceed 254 characters.");
    }
}