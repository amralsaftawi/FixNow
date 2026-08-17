namespace FixNow.Application.Features.CustomerRatings.Queries.GetCustomerReviews;

public sealed record CustomerReviewDto(
    Guid CustomerRatingId,
    Guid JobId,
    int Rating,
    string? Comment,
    DateTimeOffset CreatedAtUtc);

public sealed record GetCustomerReviewsResponse(
    IReadOnlyCollection<CustomerReviewDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
