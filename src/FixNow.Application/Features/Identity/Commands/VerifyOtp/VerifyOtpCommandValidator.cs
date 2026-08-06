using FluentValidation;

namespace FixNow.Application.Features.Identity.Commands.VerifyOtp;

public sealed class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.Identifier.Required")
            .MaximumLength(256)
            .WithErrorCode("Identity.Identifier.TooLong");

        RuleFor(x => x.Otp)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.Otp.Required")
            .Length(6)
            .WithErrorCode("Identity.Otp.InvalidLength")
            .Matches(@"^\d{6}$")
            .WithErrorCode("Identity.Otp.InvalidFormat");

        RuleFor(x => x.Purpose)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.Purpose.Required");
    }
}
