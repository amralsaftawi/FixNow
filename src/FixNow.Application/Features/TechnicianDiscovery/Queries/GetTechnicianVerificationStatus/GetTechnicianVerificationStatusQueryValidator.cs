using FluentValidation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianVerificationStatus;

public sealed class GetTechnicianVerificationStatusQueryValidator
    : AbstractValidator<GetTechnicianVerificationStatusQuery>
{
    public GetTechnicianVerificationStatusQueryValidator()
    {
        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("TechnicianDiscovery.TechnicianProfile.Required");
    }
}
