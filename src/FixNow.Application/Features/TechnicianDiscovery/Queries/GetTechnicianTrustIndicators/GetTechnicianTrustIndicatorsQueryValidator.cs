using FluentValidation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianTrustIndicators;

public sealed class GetTechnicianTrustIndicatorsQueryValidator
    : AbstractValidator<GetTechnicianTrustIndicatorsQuery>
{
    public GetTechnicianTrustIndicatorsQueryValidator()
    {
        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("TechnicianDiscovery.TechnicianProfile.Required");
    }
}
