using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RemoveTechnicianExperience;

public sealed class RemoveTechnicianExperienceCommandValidator
    : AbstractValidator<RemoveTechnicianExperienceCommand>
{
    public RemoveTechnicianExperienceCommandValidator()
    {
        RuleFor(x => x.ExperienceId)
            .NotEmpty()
            .WithErrorCode("TechnicianExperience.IdRequired");
    }
}
