namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianRating;

public sealed record GetTechnicianRatingResponse(
    Guid TechnicianProfileId,
    double AverageRating,
    int RatingCount);
