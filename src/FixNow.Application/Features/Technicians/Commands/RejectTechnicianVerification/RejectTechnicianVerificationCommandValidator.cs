using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RejectTechnicianVerification;

public sealed class RejectTechnicianVerificationCommandValidator
    : AbstractValidator<RejectTechnicianVerificationCommand>
{
    public RejectTechnicianVerificationCommandValidator()
    {
        ValidateTechnicianProfileId();
    }

    private void ValidateTechnicianProfileId()
    {
        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("TechnicianProfile.Id.Required");
    }
}
