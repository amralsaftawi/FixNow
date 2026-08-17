using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.CustomerRatings.Queries.GetCustomerRating;

public sealed record GetCustomerRatingQuery(
    Guid CustomerProfileId)
    : IQuery<Result<GetCustomerRatingResponse>>;
