using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RegisterTechnician;

public sealed class RegisterTechnicianCommandValidator
    : AbstractValidator<RegisterTechnicianCommand>
{
    public RegisterTechnicianCommandValidator()
    {
        ValidateYearsOfExperience();

        ValidateBio();

        ValidateNationalIdImageKey();

        ValidateServiceCategoryIds();
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

    private void ValidateServiceCategoryIds()
    {
        RuleFor(x => x.ServiceCategoryIds)
            .NotNull()
            .WithErrorCode("TechnicianService.ServiceCategoryIds.Required")
            .ForEach(id =>
                id.NotEmpty()
                    .WithErrorCode("TechnicianService.ServiceCategoryIdRequired"));

        RuleFor(x => x.ServiceCategoryIds)
            .Must(ids => ids is null || ids.Distinct().Count() == ids.Count)
            .WithErrorCode("TechnicianService.ServiceCategoryIds.Duplicate");
    }
}
