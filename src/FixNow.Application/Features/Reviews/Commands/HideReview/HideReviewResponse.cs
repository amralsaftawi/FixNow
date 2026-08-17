namespace FixNow.Application.Features.Reviews.Commands.HideReview;

public sealed record HideReviewResponse(
    Guid ReviewId,
    bool IsHidden);
