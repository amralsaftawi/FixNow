namespace FixNow.Application.Features.CustomerRatings.Queries.GetCustomerRating;

public sealed record GetCustomerRatingResponse(
    Guid CustomerProfileId,
    double AverageRating,
    int RatingCount);
