using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Reviews.Commands.CreateReview;

public sealed record CreateReviewCommand(
    Guid JobId,
    string Comment)
    : ICommand<Result<CreateReviewResponse>>;
