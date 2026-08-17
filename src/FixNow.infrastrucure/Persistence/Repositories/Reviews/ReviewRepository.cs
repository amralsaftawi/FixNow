using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Models;
using FixNow.Application.Features.Reviews.Queries.GetTechnicianReviews;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.Reviews;

public sealed class ReviewRepository(AppDbContext dbContext) : IReviewRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public Task AddAsync(
        Review review,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Reviews.AddAsync(
            review,
            cancellationToken).AsTask();
    }

    public Task<bool> ExistsByAssignmentIdAsync(
        Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Reviews
            .AsNoTracking()
            .AnyAsync(
                review => review.AssignmentId == assignmentId,
                cancellationToken);
    }

    public async Task<Review?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviews
            .FindAsync([id], cancellationToken);
    }

    public async Task<bool> IsOwnReviewAsync(
        Guid reviewId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviews
            .AsNoTracking()
            .AnyAsync(
                review =>
                    review.Id == reviewId
                    && review.CustomerProfile.UserId == userId,
                cancellationToken);
    }

    public async Task<PagedResult<TechnicianReviewDto>> GetByTechnicianIdPagedAsync(
        Guid technicianProfileId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Reviews
            .AsNoTracking()
            .Where(review =>
                review.TechnicianProfileId == technicianProfileId
                && !review.IsHidden);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(review => review.CreatedAtUtc)
            .Join(
                _dbContext.Jobs,
                review => new { review.ServiceRequestId, review.TechnicianProfileId },
                job => new { job.ServiceRequestId, job.TechnicianProfileId },
                (review, job) => new TechnicianReviewDto(
                    ReviewId: review.Id,
                    JobId: job.Id,
                    Rating: review.Rating,
                    Comment: review.Comment,
                    CreatedAtUtc: review.CreatedAtUtc))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TechnicianReviewDto>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }
}
