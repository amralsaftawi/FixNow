namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByLocation;

public sealed record LocatedTechnicianDto(
    Guid TechnicianProfileId,
    string FirstName,
    string LastName,
    string? ProfileImageKey,
    string? Bio,
    int YearsOfExperience);

public sealed record FilterTechniciansByLocationResponse(
    IReadOnlyCollection<LocatedTechnicianDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
