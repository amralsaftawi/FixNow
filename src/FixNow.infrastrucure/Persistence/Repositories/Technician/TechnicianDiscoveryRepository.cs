using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Models;

namespace FixNow.Infrastructure.Persistence.Repositories.Technician;

public sealed class TechnicianDiscoveryRepository : ITechnicianDiscoveryRepository
{
    private readonly AppDbContext _dbContext;

    public TechnicianDiscoveryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<PagedResult<(global::TechnicianProfile Entity, double DistanceInKm)>> FindNearbyAsync(
        Guid serviceCategoryId,
        decimal latitude,
        decimal longitude,
        double radiusInKm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
