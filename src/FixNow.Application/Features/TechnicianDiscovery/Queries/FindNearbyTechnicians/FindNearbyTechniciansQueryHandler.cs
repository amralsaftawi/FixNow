using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians;

public sealed class FindNearbyTechniciansQueryHandler(
    ITechnicianDiscoveryRepository technicianDiscoveryRepository)
    : IQueryHandler<FindNearbyTechniciansQuery, Result<FindNearbyTechniciansResponse>>
{
    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    public async Task<Result<FindNearbyTechniciansResponse>> Handle(
        FindNearbyTechniciansQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _technicianDiscoveryRepository.FindNearbyAsync(
            latitude: query.Latitude,
            longitude: query.Longitude,
            radiusInKm: query.RadiusInKm,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken);

        return new FindNearbyTechniciansResponse(
            Items: result.Items,
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}
