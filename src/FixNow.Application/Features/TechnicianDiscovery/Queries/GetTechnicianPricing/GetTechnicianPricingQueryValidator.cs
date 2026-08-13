using FluentValidation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPricing;

public sealed class GetTechnicianPricingQueryValidator
    : AbstractValidator<GetTechnicianPricingQuery>
{
    public GetTechnicianPricingQueryValidator()
    {
        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("TechnicianDiscovery.TechnicianProfile.Required");
    }
}
