using FluentValidation;

namespace FixNow.Application.Features.Identity.Commands.SendOtp;

public sealed class SendOtpCommandValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Identity.Identifier.Required")
            .MaximumLength(320)
            .WithErrorCode("Identity.Identifier.TooLong")
            .Must(BeEmailOrPhoneNumber)
            .WithErrorCode("Identity.Identifier.Invalid");
    }

    private static bool BeEmailOrPhoneNumber(string identifier)
        => !Email.Create(identifier).IsError ||
           !PhoneNumber.Create(identifier).IsError;
}
