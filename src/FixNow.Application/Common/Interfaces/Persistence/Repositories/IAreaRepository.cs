namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface IAreaRepository
{
    Task<List<Area>> GetByCityIdAsync(
        int cityId,
        CancellationToken cancellationToken = default);

    Task<Area?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<Area?> GetWithCityByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Area area,
        CancellationToken cancellationToken = default);
}
