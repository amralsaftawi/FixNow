namespace FixNow.Contracts.Responses;

public sealed record TechnicianReviewItemResponse(
    Guid ReviewId,
    Guid JobId,
    int Rating,
    string? Comment,
    DateTimeOffset CreatedAtUtc);

public sealed record TechnicianReviewsResponse(
    IReadOnlyCollection<TechnicianReviewItemResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
