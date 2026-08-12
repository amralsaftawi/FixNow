using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Models;
using FixNow.Application.Features.ServiceCategories.Queries.FilterServiceCategories;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.ServiceCategory;

public sealed class ServiceCategoryRepository : IServiceCategoryRepository
{
    private readonly AppDbContext _dbContext;

    public ServiceCategoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.ServiceCategories
            .AsNoTracking()
            .AnyAsync(
                category => category.Name == name.Trim(),
                cancellationToken);
    }

    public async Task AddAsync(
        global::ServiceCategory serviceCategory,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.ServiceCategories.AddAsync(
            serviceCategory,
            cancellationToken);
    }

    public Task<global::ServiceCategory?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.ServiceCategories
            .FirstOrDefaultAsync(
                category => category.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<global::ServiceCategory>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        if (ids.Count == 0)
        {
            return [];
        }

        return await _dbContext.ServiceCategories
            .AsNoTracking()
            .Where(category => ids.Contains(category.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<global::ServiceCategory>> GetPagedAsync(
        string? search,
        bool? isActive,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.ServiceCategories
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();
            query = query.Where(category => category.Name.Contains(normalizedSearch));
        }

        if (isActive.HasValue)
        {
            query = query.Where(category => category.IsActive == isActive.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(category => category.DisplayOrder)
            .ThenBy(category => category.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<global::ServiceCategory>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }

    public async Task<PagedResult<global::ServiceCategory>> GetActivePagedAsync(
        string? search,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.ServiceCategories
            .AsNoTracking()
            .Where(category => category.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();
            query = query.Where(category => category.Name.Contains(normalizedSearch));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(category => category.DisplayOrder)
            .ThenBy(category => category.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<global::ServiceCategory>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }

    public async Task<PagedResult<global::ServiceCategory>> SearchActivePagedAsync(
        string? search,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.ServiceCategories
            .AsNoTracking()
            .Where(category => category.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();

            query = query.Where(category =>
                category.Name.Contains(normalizedSearch)
                || category.Description.Contains(normalizedSearch));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(category => category.DisplayOrder)
            .ThenBy(category => category.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<global::ServiceCategory>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }

    public async Task<PagedResult<global::ServiceCategory>> FilterActivePagedAsync(
        string? search,
        decimal? minPrice,
        decimal? maxPrice,
        ServiceCategorySortBy sortBy,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.ServiceCategories
            .AsNoTracking()
            .Where(category => category.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();

            query = query.Where(category =>
                category.Name.Contains(normalizedSearch)
                || category.Description.Contains(normalizedSearch));
        }

        if (minPrice.HasValue)
        {
            var minimumPrice = minPrice.Value;

            query = query.Where(category =>
                category.Price != null
                && category.Price.Value >= minimumPrice);
        }

        if (maxPrice.HasValue)
        {
            var maximumPrice = maxPrice.Value;

            query = query.Where(category =>
                category.Price != null
                && category.Price.Value <= maximumPrice);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await OrderByCategory(query, sortBy)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<global::ServiceCategory>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }

    private static IOrderedQueryable<global::ServiceCategory> OrderByCategory(
        IQueryable<global::ServiceCategory> query,
        ServiceCategorySortBy sortBy)
    {
        return sortBy switch
        {
            ServiceCategorySortBy.NameAsc =>
                query.OrderBy(category => category.Name),
            ServiceCategorySortBy.NameDesc =>
                query.OrderByDescending(category => category.Name),
            ServiceCategorySortBy.PriceAsc =>
                query
                    .OrderBy(category => category.Price == null)
                    .ThenBy(category => category.Price!.Value),
            ServiceCategorySortBy.PriceDesc =>
                query
                    .OrderBy(category => category.Price == null)
                    .ThenByDescending(category => category.Price!.Value),
            ServiceCategorySortBy.Newest =>
                query.OrderByDescending(category => category.CreatedAtUtc),
            _ =>
                query
                    .OrderBy(category => category.DisplayOrder)
                    .ThenBy(category => category.Name),
        };
    }
}
