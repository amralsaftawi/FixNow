using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.Geographic;

public sealed class AreaRepository(AppDbContext dbContext) : IAreaRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Area>> GetByCityIdAsync(
        int cityId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Areas
            .AsNoTracking()
            .Where(area => area.CityId == cityId)
            .OrderBy(area => area.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Area?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Areas
            .AsNoTracking()
            .FirstOrDefaultAsync(
                area => area.Id == id,
                cancellationToken);
    }

    public async Task<Area?> GetWithCityByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Areas
            .AsNoTracking()
            .Include(area => area.City)
            .FirstOrDefaultAsync(
                area => area.Id == id,
                cancellationToken);
    }

    public Task<bool> ExistsByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Areas
            .AsNoTracking()
            .AnyAsync(
                area => area.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(
        Area area,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Areas.AddAsync(
            area,
            cancellationToken);
    }
}
