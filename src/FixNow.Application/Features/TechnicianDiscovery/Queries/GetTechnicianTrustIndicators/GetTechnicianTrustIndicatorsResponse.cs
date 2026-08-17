namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianTrustIndicators;

public sealed record GetTechnicianTrustIndicatorsResponse(
    Guid TechnicianProfileId,
    bool IsVerified,
    bool IsProfileComplete,
    bool IsActive,
    int YearsOfExperience,
    double AverageRating,
    int TotalRatings);
