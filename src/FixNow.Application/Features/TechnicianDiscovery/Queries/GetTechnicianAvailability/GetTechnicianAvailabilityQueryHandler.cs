using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianAvailability;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianAvailability;

public sealed class GetTechnicianAvailabilityQueryHandler(
    ITechnicianDiscoveryRepository technicianDiscoveryRepository)
    : IQueryHandler<
        GetTechnicianAvailabilityQuery,
        Result<TechnicianAvailabilitySettingsResponse>>
{
    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    public async Task<Result<TechnicianAvailabilitySettingsResponse>> Handle(
        GetTechnicianAvailabilityQuery query,
        CancellationToken cancellationToken)
    {
        var availability =
            await _technicianDiscoveryRepository.GetAvailabilityByTechnicianAsync(
                technicianProfileId: query.TechnicianProfileId,
                cancellationToken: cancellationToken);

        if (availability is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return availability;
    }
}
