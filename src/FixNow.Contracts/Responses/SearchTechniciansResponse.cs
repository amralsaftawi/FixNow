namespace FixNow.Contracts.Responses;

public sealed record TechnicianSearchResultResponse(
    Guid TechnicianProfileId,
    string FirstName,
    string LastName,
    string? ProfileImageKey,
    string? Bio,
    int YearsOfExperience);

public sealed record SearchTechniciansResponse(
    IReadOnlyCollection<TechnicianSearchResultResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
