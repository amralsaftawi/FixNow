namespace FixNow.Contracts.Responses;

public sealed record ServiceTechnicianResponse(
    Guid TechnicianProfileId,
    string FirstName,
    string LastName,
    string? ProfileImageKey,
    string? Bio,
    int YearsOfExperience);

public sealed record FilterTechniciansByServiceResponse(
    IReadOnlyCollection<ServiceTechnicianResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
