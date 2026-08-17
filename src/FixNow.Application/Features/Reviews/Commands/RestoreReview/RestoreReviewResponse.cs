namespace FixNow.Application.Features.Reviews.Commands.RestoreReview;

public sealed record RestoreReviewResponse(
    Guid ReviewId,
    bool IsHidden);
