using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Models;

namespace FixNow.Infrastructure.Persistence.Repositories.Technician;

public sealed class TechnicianProfileRepository : ITechnicianProfileRepository
{
    private readonly AppDbContext _dbContext;

    public TechnicianProfileRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<global::TechnicianProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<global::TechnicianProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<global::TechnicianProfile>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(global::TechnicianProfile technicianProfile)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(global::TechnicianProfile technicianProfile, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
