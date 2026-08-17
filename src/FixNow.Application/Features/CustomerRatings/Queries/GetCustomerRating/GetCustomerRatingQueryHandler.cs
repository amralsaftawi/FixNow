using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.CustomerRatings.Queries.GetCustomerRating;

public sealed class GetCustomerRatingQueryHandler(
    ICustomerRatingRepository customerRatingRepository)
    : IQueryHandler<GetCustomerRatingQuery, Result<GetCustomerRatingResponse>>
{
    private readonly ICustomerRatingRepository _customerRatingRepository =
        customerRatingRepository;

    public async Task<Result<GetCustomerRatingResponse>> Handle(
        GetCustomerRatingQuery query,
        CancellationToken cancellationToken)
    {
        var exists =
            await _customerRatingRepository.ExistsByCustomerProfileIdAsync(
                customerProfileId: query.CustomerProfileId,
                cancellationToken: cancellationToken);

        var summary =
            await _customerRatingRepository.GetRatingSummaryByCustomerAsync(
                customerProfileId: query.CustomerProfileId,
                cancellationToken: cancellationToken);

        if (summary is null && !exists)
        {
            return CustomerProfileErrors.NotFound;
        }

        return new GetCustomerRatingResponse(
            CustomerProfileId: query.CustomerProfileId,
            AverageRating: summary?.AverageRating ?? 0,
            RatingCount: summary?.RatingCount ?? 0);
    }
}
