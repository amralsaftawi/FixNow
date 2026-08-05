using FluentValidation;

namespace FixNow.Application.Features.Identity.Commands.Logout;

public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.RefreshToken.Required")
            .MaximumLength(500)
            .WithErrorCode("Identity.RefreshToken.TooLong");
    }
}
