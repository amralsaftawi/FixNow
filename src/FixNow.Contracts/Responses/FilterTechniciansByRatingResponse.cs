namespace FixNow.Contracts.Responses;

public sealed record RatedTechnicianResponse(
    Guid TechnicianProfileId,
    string FirstName,
    string LastName,
    string? ProfileImageKey,
    string? Bio,
    int YearsOfExperience,
    double AverageRating);

public sealed record FilterTechniciansByRatingResponse(
    IReadOnlyCollection<RatedTechnicianResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
