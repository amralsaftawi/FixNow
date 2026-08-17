namespace FixNow.Application.Features.Reviews.Queries.GetTechnicianReviews;

public sealed record TechnicianReviewDto(
    Guid ReviewId,
    Guid JobId,
    int Rating,
    string? Comment,
    DateTimeOffset CreatedAtUtc);

public sealed record GetTechnicianReviewsResponse(
    IReadOnlyCollection<TechnicianReviewDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
