using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianExperience;

public sealed class UpdateTechnicianExperienceCommandValidator
    : AbstractValidator<UpdateTechnicianExperienceCommand>
{
    public UpdateTechnicianExperienceCommandValidator()
    {
        ValidateExperienceId();

        ValidateCompanyName();

        ValidatePosition();

        ValidateDescription();

        ValidateStartDate();

        ValidateEndDate();
    }

    private void ValidateExperienceId()
    {
        RuleFor(x => x.ExperienceId)
            .NotEmpty()
            .WithErrorCode("TechnicianExperience.IdRequired");
    }

    private void ValidateCompanyName()
    {
        RuleFor(x => x.CompanyName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("TechnicianExperience.CompanyNameRequired")
            .MaximumLength(150)
            .WithErrorCode("TechnicianExperience.CompanyNameTooLong");
    }

    private void ValidatePosition()
    {
        RuleFor(x => x.Position)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("TechnicianExperience.PositionRequired")
            .MaximumLength(150)
            .WithErrorCode("TechnicianExperience.PositionTooLong");
    }

    private void ValidateDescription()
    {
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithErrorCode("TechnicianExperience.DescriptionTooLong");
    }

    private void ValidateStartDate()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithErrorCode("TechnicianExperience.StartDateRequired");
    }

    private void ValidateEndDate()
    {
        RuleFor(x => x.EndDate)
            .Must((command, endDate) => endDate is null || endDate > command.StartDate)
            .WithErrorCode("TechnicianExperience.EndDateBeforeStartDate");
    }
}
