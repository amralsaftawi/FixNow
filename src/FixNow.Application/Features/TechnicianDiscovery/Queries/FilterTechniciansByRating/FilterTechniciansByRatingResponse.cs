namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByRating;

public sealed record RatedTechnicianDto(
    Guid TechnicianProfileId,
    string FirstName,
    string LastName,
    string? ProfileImageKey,
    string? Bio,
    int YearsOfExperience,
    double AverageRating);

public sealed record FilterTechniciansByRatingResponse(
    IReadOnlyCollection<RatedTechnicianDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
