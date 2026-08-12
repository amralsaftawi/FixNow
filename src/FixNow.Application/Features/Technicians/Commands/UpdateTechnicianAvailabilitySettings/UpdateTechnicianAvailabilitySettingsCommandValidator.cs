using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAvailabilitySettings;

public sealed class UpdateTechnicianAvailabilitySettingsCommandValidator
    : AbstractValidator<UpdateTechnicianAvailabilitySettingsCommand>
{
    public UpdateTechnicianAvailabilitySettingsCommandValidator()
    {
        ValidateStatus();

        ValidateWorkingDays();

        ValidateVacation();
    }

    private void ValidateStatus()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithErrorCode("TechnicianAvailability.Status.Invalid");
    }

    private void ValidateWorkingDays()
    {
        RuleFor(x => x.WorkingDays)
            .NotNull()
            .WithErrorCode("TechnicianAvailability.WorkingDay.Required");

        RuleForEach(x => x.WorkingDays)
            .ChildRules(workingDay =>
            {
                workingDay.RuleFor(x => x.Day)
                    .IsInEnum()
                    .WithErrorCode("TechnicianAvailability.WorkingDay.InvalidDay");

                workingDay.RuleFor(x => x.EndTime)
                    .GreaterThan(x => x.StartTime)
                    .WithErrorCode(
                        "TechnicianAvailability.WorkingDay.EndTimeNotAfterStartTime");
            });

        RuleFor(x => x.WorkingDays)
            .Must(workingDays =>
                workingDays is null
                || workingDays
                    .GroupBy(x => x.Day)
                    .All(group => group.Count() == 1))
            .WithErrorCode("TechnicianAvailability.DuplicateWorkingDay");
    }

    private void ValidateVacation()
    {
        When(
            x => x.VacationStartDate is not null
                || x.VacationEndDate is not null,
            () =>
            {
                RuleFor(x => x.VacationStartDate)
                    .NotNull()
                    .WithErrorCode("TechnicianAvailability.VacationDatesBothRequired");

                RuleFor(x => x.VacationEndDate)
                    .NotNull()
                    .WithErrorCode("TechnicianAvailability.VacationDatesBothRequired");

                RuleFor(x => x.VacationEndDate)
                    .GreaterThanOrEqualTo(x => x.VacationStartDate!.Value)
                    .WithErrorCode("TechnicianAvailability.InvalidVacationRange");
            });
    }
}
