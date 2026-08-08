using FluentValidation;

namespace FixNow.Application.Features.Identity.Commands.ResendOtp;

public sealed class ResendOtpCommandValidator
    : AbstractValidator<ResendOtpCommand>
{
    public ResendOtpCommandValidator()
    {
        ValidateIdentifier();

        ValidatePurpose();
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

    private void ValidatePurpose()
    {
        RuleFor(x => x.Purpose)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.OtpPurpose.Required")
            .MaximumLength(50)
            .WithErrorCode("Identity.OtpPurpose.TooLong");
    }
}