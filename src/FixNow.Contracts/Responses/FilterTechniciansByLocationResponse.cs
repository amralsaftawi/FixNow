namespace FixNow.Contracts.Responses;

public sealed record LocatedTechnicianResponse(
    Guid TechnicianProfileId,
    string FirstName,
    string LastName,
    string? ProfileImageKey,
    string? Bio,
    int YearsOfExperience);

public sealed record FilterTechniciansByLocationResponse(
    IReadOnlyCollection<LocatedTechnicianResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
