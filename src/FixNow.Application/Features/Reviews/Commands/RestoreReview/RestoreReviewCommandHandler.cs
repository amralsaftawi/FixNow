using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Reviews.Commands.RestoreReview;

public sealed class RestoreReviewCommandHandler(
    IReviewRepository reviewRepository)
    : ICommandHandler<RestoreReviewCommand, Result<RestoreReviewResponse>>
{
    private readonly IReviewRepository _reviewRepository = reviewRepository;

    public async Task<Result<RestoreReviewResponse>> Handle(
        RestoreReviewCommand command,
        CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(
            command.ReviewId,
            cancellationToken);

        if (review is null)
            return ReviewErrors.NotFound;

        var restoreResult = review.Restore();

        if (restoreResult.IsError)
            return restoreResult.Errors;

        return new RestoreReviewResponse(
            ReviewId: review.Id,
            IsHidden: review.IsHidden);
    }
}
