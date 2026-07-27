using FixNow.Application.Common.Models;

namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface ITechnicianProfileRepository
{
    Task<bool> ExistsByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<TechnicianProfile?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<TechnicianProfile?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TechnicianProfile>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    void Update(TechnicianProfile technicianProfile);

    Task AddAsync(
        TechnicianProfile technicianProfile,
        CancellationToken cancellationToken = default);
}