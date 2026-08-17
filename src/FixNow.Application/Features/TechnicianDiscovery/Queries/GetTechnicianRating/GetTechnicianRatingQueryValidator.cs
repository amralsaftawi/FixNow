using FluentValidation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianRating;

public sealed class GetTechnicianRatingQueryValidator
    : AbstractValidator<GetTechnicianRatingQuery>
{
    public GetTechnicianRatingQueryValidator()
    {
        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("TechnicianDiscovery.TechnicianProfile.Required");
    }
}
