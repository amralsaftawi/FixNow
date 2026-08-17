namespace FixNow.Contracts.Responses;

public sealed record CustomerReviewItemResponse(
    Guid CustomerRatingId,
    Guid JobId,
    int Rating,
    string? Comment,
    DateTimeOffset CreatedAtUtc);

public sealed record CustomerReviewsResponse(
    IReadOnlyCollection<CustomerReviewItemResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
