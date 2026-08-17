using FixNow.Application.Common.Models;
using FixNow.Application.Features.Reviews.Queries.GetTechnicianReviews;

namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface IReviewRepository
{
    Task AddAsync(
        Review review,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByAssignmentIdAsync(
        Guid assignmentId,
        CancellationToken cancellationToken = default);

    Task<Review?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> IsOwnReviewAsync(
        Guid reviewId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TechnicianReviewDto>> GetByTechnicianIdPagedAsync(
        Guid technicianProfileId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
