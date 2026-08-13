namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByService;

public sealed record ServiceTechnicianDto(
    Guid TechnicianProfileId,
    string FirstName,
    string LastName,
    string? ProfileImageKey,
    string? Bio,
    int YearsOfExperience);

public sealed record FilterTechniciansByServiceResponse(
    IReadOnlyCollection<ServiceTechnicianDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
