using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Reviews.Commands.RestoreReview;

public sealed record RestoreReviewCommand(
    Guid ReviewId)
    : ICommand<Result<RestoreReviewResponse>>;
