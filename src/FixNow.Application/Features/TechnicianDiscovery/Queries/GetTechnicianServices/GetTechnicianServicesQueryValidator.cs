using FluentValidation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianServices;

public sealed class GetTechnicianServicesQueryValidator
    : AbstractValidator<GetTechnicianServicesQuery>
{
    public GetTechnicianServicesQueryValidator()
    {
        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("TechnicianDiscovery.TechnicianProfile.Required");
    }
}
