using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.Geographic;

public sealed class CityRepository(AppDbContext dbContext) : ICityRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<City>> GetByCountryIdAsync(
        int countryId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Cities
            .AsNoTracking()
            .Where(city => city.CountryId == countryId)
            .OrderBy(city => city.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<City?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(
                city => city.Id == id,
                cancellationToken);
    }

    public Task<bool> ExistsByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Cities
            .AsNoTracking()
            .AnyAsync(
                city => city.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(
        City city,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Cities.AddAsync(
            city,
            cancellationToken);
    }
}
