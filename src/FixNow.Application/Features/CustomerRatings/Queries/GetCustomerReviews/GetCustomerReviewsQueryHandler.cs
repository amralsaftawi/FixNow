using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.CustomerRatings.Queries.GetCustomerReviews;

public sealed class GetCustomerReviewsQueryHandler(
    ICustomerRatingRepository customerRatingRepository)
    : IQueryHandler<GetCustomerReviewsQuery, Result<GetCustomerReviewsResponse>>
{
    private readonly ICustomerRatingRepository _customerRatingRepository =
        customerRatingRepository;

    public async Task<Result<GetCustomerReviewsResponse>> Handle(
        GetCustomerReviewsQuery query,
        CancellationToken cancellationToken)
    {
        var exists =
            await _customerRatingRepository.ExistsByCustomerProfileIdAsync(
                customerProfileId: query.CustomerProfileId,
                cancellationToken: cancellationToken);

        if (!exists)
        {
            return CustomerProfileErrors.NotFound;
        }

        var pagedResult =
            await _customerRatingRepository.GetByCustomerIdPagedAsync(
                customerProfileId: query.CustomerProfileId,
                pageNumber: query.PageNumber,
                pageSize: query.PageSize,
                cancellationToken: cancellationToken);

        return new GetCustomerReviewsResponse(
            Items: pagedResult.Items,
            PageNumber: pagedResult.PageNumber,
            PageSize: pagedResult.PageSize,
            TotalCount: pagedResult.TotalCount,
            TotalPages: pagedResult.TotalPages);
    }
}
