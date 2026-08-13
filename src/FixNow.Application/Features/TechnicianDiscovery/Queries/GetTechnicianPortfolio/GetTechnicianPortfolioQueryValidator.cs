using FluentValidation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPortfolio;

public sealed class GetTechnicianPortfolioQueryValidator
    : AbstractValidator<GetTechnicianPortfolioQuery>
{
    public GetTechnicianPortfolioQueryValidator()
    {
        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("TechnicianDiscovery.TechnicianProfile.Required");
    }
}
