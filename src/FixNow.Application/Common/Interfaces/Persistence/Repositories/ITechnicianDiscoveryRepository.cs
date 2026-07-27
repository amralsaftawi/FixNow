using FixNow.Application.Common.Models;


namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface ITechnicianDiscoveryRepository
{
    Task<PagedResult<(TechnicianProfile Entity, double DistanceInKm)>> FindNearbyAsync(
        Guid serviceCategoryId,
        decimal latitude,
        decimal longitude,
        double radiusInKm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}