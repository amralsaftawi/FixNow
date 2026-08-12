using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.AddTechnicianExperience;

public sealed class AddTechnicianExperienceCommandValidator
    : AbstractValidator<AddTechnicianExperienceCommand>
{
    public AddTechnicianExperienceCommandValidator()
    {
        ValidateCompanyName();

        ValidatePosition();

        ValidateDescription();

        ValidateStartDate();

        ValidateEndDate();
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
