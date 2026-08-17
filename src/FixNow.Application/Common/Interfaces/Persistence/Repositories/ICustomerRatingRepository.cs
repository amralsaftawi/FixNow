using FixNow.Application.Common.Models;
using FixNow.Application.Features.CustomerRatings.Queries.GetCustomerReviews;

namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface ICustomerRatingRepository
{
    Task AddAsync(
        CustomerRating customerRating,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByJobIdAsync(
        Guid jobId,
        CancellationToken cancellationToken = default);

    Task<RatingSummary?> GetRatingSummaryByCustomerAsync(
        Guid customerProfileId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCustomerProfileIdAsync(
        Guid customerProfileId,
        CancellationToken cancellationToken = default);

    Task<PagedResult<CustomerReviewDto>> GetByCustomerIdPagedAsync(
        Guid customerProfileId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
