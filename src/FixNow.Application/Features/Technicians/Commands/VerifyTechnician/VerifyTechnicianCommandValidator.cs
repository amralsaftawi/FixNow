using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.VerifyTechnician;

public sealed class VerifyTechnicianCommandValidator
    : AbstractValidator<VerifyTechnicianCommand>
{
    public VerifyTechnicianCommandValidator()
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
