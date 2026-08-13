namespace FixNow.Contracts.Responses;

public sealed record NearbyTechnicianResponse(
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
    IReadOnlyCollection<NearbyTechnicianResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
