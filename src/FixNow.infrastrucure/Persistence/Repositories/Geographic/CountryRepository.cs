using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.Geographic;

public sealed class CountryRepository(AppDbContext dbContext) : ICountryRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Country>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Countries
            .AsNoTracking()
            .OrderBy(country => country.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Country?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(
                country => country.Id == id,
                cancellationToken);
    }

    public Task<bool> ExistsByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Countries
            .AsNoTracking()
            .AnyAsync(
                country => country.Id == id,
                cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Countries
            .AsNoTracking()
            .AnyAsync(
                country => country.Name == name.Trim(),
                cancellationToken);
    }

    public async Task AddAsync(
        Country country,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Countries.AddAsync(
            country,
            cancellationToken);
    }
}
