namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface ICountryRepository
{
    Task<List<Country>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Country?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Country country,
        CancellationToken cancellationToken = default);
}
