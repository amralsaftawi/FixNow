namespace FixNow.Application.Features.TechnicianSearch.Queries.SearchTechnicians;

public sealed record TechnicianSearchResultDto(
    Guid TechnicianProfileId,
    string FirstName,
    string LastName,
    string? ProfileImageKey,
    string? Bio,
    int YearsOfExperience);

public sealed record SearchTechniciansResponse(
    IReadOnlyCollection<TechnicianSearchResultDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
