namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians;

public sealed record NearbyTechnicianDto(
    Guid TechnicianProfileId,
    Guid UserId,
    string FirstName,
    string LastName,
    string? ProfileImageKey,
    string? Bio,
    int YearsOfExperience,
    double DistanceInKm,
    decimal? Latitude,
    decimal? Longitude);

public sealed record FindNearbyTechniciansResponse(
    IReadOnlyCollection<NearbyTechnicianDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);