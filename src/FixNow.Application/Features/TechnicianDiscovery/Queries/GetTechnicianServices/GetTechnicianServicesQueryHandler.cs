using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianServices;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianServices;

public sealed class GetTechnicianServicesQueryHandler(
    ITechnicianDiscoveryRepository technicianDiscoveryRepository)
    : IQueryHandler<GetTechnicianServicesQuery, Result<TechnicianServicesResponse>>
{
    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    public async Task<Result<TechnicianServicesResponse>> Handle(
        GetTechnicianServicesQuery query,
        CancellationToken cancellationToken)
    {
        var services = await _technicianDiscoveryRepository.GetServicesByTechnicianAsync(
            technicianProfileId: query.TechnicianProfileId,
            cancellationToken: cancellationToken);

        if (services is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return new TechnicianServicesResponse(
            Items: services);
    }
}
