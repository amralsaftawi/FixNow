using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.CustomerRatings.Queries.GetCustomerReviews;

public sealed record GetCustomerReviewsQuery(
    Guid CustomerProfileId,
    int PageNumber = 1,
    int PageSize = 20)
    : IQuery<Result<GetCustomerReviewsResponse>>;
