using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPricing;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPricing;

public sealed class GetTechnicianPricingQueryHandler(
    ITechnicianDiscoveryRepository technicianDiscoveryRepository)
    : IQueryHandler<GetTechnicianPricingQuery, Result<TechnicianPricingResponse>>
{
    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    public async Task<Result<TechnicianPricingResponse>> Handle(
        GetTechnicianPricingQuery query,
        CancellationToken cancellationToken)
    {
        var pricing = await _technicianDiscoveryRepository.GetPricingByTechnicianAsync(
            technicianProfileId: query.TechnicianProfileId,
            cancellationToken: cancellationToken);

        if (pricing is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return new TechnicianPricingResponse(
            Items: pricing);
    }
}
