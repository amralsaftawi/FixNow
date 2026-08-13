using FluentValidation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianAvailability;

public sealed class GetTechnicianAvailabilityQueryValidator
    : AbstractValidator<GetTechnicianAvailabilityQuery>
{
    public GetTechnicianAvailabilityQueryValidator()
    {
        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("TechnicianDiscovery.TechnicianProfile.Required");
    }
}
