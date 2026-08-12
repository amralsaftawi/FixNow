using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAccountStatus;

public sealed class UpdateTechnicianAccountStatusCommandValidator
    : AbstractValidator<UpdateTechnicianAccountStatusCommand>
{
    public UpdateTechnicianAccountStatusCommandValidator()
    {
        ValidateTechnicianProfileId();
        ValidateStatus();
    }

    private void ValidateTechnicianProfileId()
    {
        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("TechnicianProfile.Id.Required");
    }

    private void ValidateStatus()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithErrorCode("User.AccountStatus.Invalid")
            .Must(IsAdministrativeStatus)
            .WithErrorCode("User.AccountStatus.Invalid");
    }

    private static bool IsAdministrativeStatus(AccountStatus status)
        => status is AccountStatus.Active
            or AccountStatus.Suspended
            or AccountStatus.Deactivated;
}
