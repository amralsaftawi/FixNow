using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Reviews.Commands.HideReview;

public sealed class HideReviewCommandHandler(
    IReviewRepository reviewRepository)
    : ICommandHandler<HideReviewCommand, Result<HideReviewResponse>>
{
    private readonly IReviewRepository _reviewRepository = reviewRepository;

    public async Task<Result<HideReviewResponse>> Handle(
        HideReviewCommand command,
        CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(
            command.ReviewId,
            cancellationToken);

        if (review is null)
            return ReviewErrors.NotFound;

        var hideResult = review.Hide();

        if (hideResult.IsError)
            return hideResult.Errors;

        return new HideReviewResponse(
            ReviewId: review.Id,
            IsHidden: review.IsHidden);
    }
}
