namespace FixNow.Contracts.Responses;

public sealed record TechnicianRatingResponse(
    Guid TechnicianProfileId,
    double AverageRating,
    int RatingCount);
