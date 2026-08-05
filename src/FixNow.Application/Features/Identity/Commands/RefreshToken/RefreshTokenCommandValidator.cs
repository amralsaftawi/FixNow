using FluentValidation;

namespace FixNow.Application.Features.Identity.Commands.RefreshToken;

public sealed class RefreshTokenCommandValidator
    : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.RefreshToken.Required")
            .MaximumLength(500)
            .WithErrorCode("Identity.RefreshToken.TooLong");
    }
}