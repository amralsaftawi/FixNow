namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface ICityRepository
{
    Task<List<City>> GetByCountryIdAsync(
        int countryId,
        CancellationToken cancellationToken = default);

    Task<City?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        City city,
        CancellationToken cancellationToken = default);
}
