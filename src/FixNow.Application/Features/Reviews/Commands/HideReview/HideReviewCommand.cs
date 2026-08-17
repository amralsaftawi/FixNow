using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Reviews.Commands.HideReview;

public sealed record HideReviewCommand(
    Guid ReviewId)
    : ICommand<Result<HideReviewResponse>>;
