using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Models;
using FixNow.Application.Features.CustomerRatings.Queries.GetCustomerReviews;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.CustomerRatings;

public sealed class CustomerRatingRepository(AppDbContext dbContext) : ICustomerRatingRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public Task AddAsync(
        CustomerRating customerRating,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.CustomerRatings.AddAsync(
            customerRating,
            cancellationToken).AsTask();
    }

    public Task<bool> ExistsByJobIdAsync(
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.CustomerRatings
            .AsNoTracking()
            .AnyAsync(
                rating => rating.JobId == jobId,
                cancellationToken);
    }

    public async Task<RatingSummary?> GetRatingSummaryByCustomerAsync(
        Guid customerProfileId,
        CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.CustomerRatings
            .AsNoTracking()
            .Where(rating =>
                rating.CustomerProfileId == customerProfileId)
            .GroupBy(_ => 1)
            .Select(group => new RatingSummary(
                AverageRating: group.Average(r => (int)r.Rating),
                RatingCount: group.Count()))
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public Task<bool> ExistsByCustomerProfileIdAsync(
        Guid customerProfileId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.CustomerRatings
            .AsNoTracking()
            .AnyAsync(
                rating => rating.CustomerProfileId == customerProfileId,
                cancellationToken);
    }

    public async Task<PagedResult<CustomerReviewDto>> GetByCustomerIdPagedAsync(
        Guid customerProfileId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.CustomerRatings
            .AsNoTracking()
            .Where(rating =>
                rating.CustomerProfileId == customerProfileId);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(rating => rating.CreatedAtUtc)
            .Select(rating => new CustomerReviewDto(
                CustomerRatingId: rating.Id,
                JobId: rating.JobId,
                Rating: rating.Rating,
                Comment: rating.Comment,
                CreatedAtUtc: rating.CreatedAtUtc))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<CustomerReviewDto>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }
}
