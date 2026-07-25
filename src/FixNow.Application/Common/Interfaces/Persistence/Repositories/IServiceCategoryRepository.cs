

using FixNow.Application.Common.Models;

public interface IServiceCategoryRepository
{
    Task<bool> ExistsByNameAsync(string name,CancellationToken cancellationToken = default);

   Task AddAsync(ServiceCategory serviceCategory,CancellationToken cancellationToken = default);

   Task<ServiceCategory?> GetByIdAsync( Guid id, CancellationToken cancellationToken = default);

   Task<PagedResult<ServiceCategory>> GetPagedAsync(
    string? search,
    bool? isActive,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default);


    Task<PagedResult<ServiceCategory>> GetActivePagedAsync(
    string? search,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default);
} 