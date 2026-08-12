using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianProfile;

public sealed class UpdateTechnicianProfileCommandValidator
    : AbstractValidator<UpdateTechnicianProfileCommand>
{
    public UpdateTechnicianProfileCommandValidator()
    {
        ValidateYearsOfExperience();

        ValidateBio();

        ValidateNationalIdImageKey();
    }

    private void ValidateYearsOfExperience()
    {
        RuleFor(x => x.YearsOfExperience)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("TechnicianProfile.YearsOfExperience.Invalid");
    }

    private void ValidateBio()
    {
        RuleFor(x => x.Bio)
            .MaximumLength(1000)
            .WithErrorCode("TechnicianProfile.Bio.TooLong");
    }

    private void ValidateNationalIdImageKey()
    {
        RuleFor(x => x.NationalIdImageKey)
            .MaximumLength(500)
            .WithErrorCode("TechnicianProfile.NationalIdImageKey.TooLong");
    }
}
