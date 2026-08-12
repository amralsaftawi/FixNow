using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetTechnicianAccountStatus;

public sealed class GetTechnicianAccountStatusQueryValidator
    : AbstractValidator<GetTechnicianAccountStatusQuery>
{
    public GetTechnicianAccountStatusQueryValidator()
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
