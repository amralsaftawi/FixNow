namespace FixNow.Contracts.Responses;

public sealed record TechnicianTrustIndicatorsResponse(
    Guid TechnicianProfileId,
    bool IsVerified,
    bool IsProfileComplete,
    bool IsActive,
    int YearsOfExperience,
    double AverageRating,
    int TotalRatings);
