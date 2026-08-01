using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Models;

namespace FixNow.Infrastructure.Persistence.Repositories.ServiceCategory;

public sealed class ServiceCategoryRepository : IServiceCategoryRepository
{
    private readonly AppDbContext _dbContext;

    public ServiceCategoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(global::ServiceCategory serviceCategory, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<global::ServiceCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<global::ServiceCategory>> GetPagedAsync(string? search, bool? isActive, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<global::ServiceCategory>> GetActivePagedAsync(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
