

using FixNow.Application.Common.Models;
using FixNow.Application.Features.ServiceCategories.Queries.FilterServiceCategories;

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

    Task<PagedResult<ServiceCategory>> SearchActivePagedAsync(
    string? search,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default);

    Task<PagedResult<ServiceCategory>> FilterActivePagedAsync(
    string? search,
    decimal? minPrice,
    decimal? maxPrice,
    ServiceCategorySortBy sortBy,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default);
} 