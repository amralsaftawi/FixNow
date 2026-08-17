namespace FixNow.Contracts.Responses;

public sealed record CustomerRatingResponse(
    Guid CustomerProfileId,
    double AverageRating,
    int RatingCount);
