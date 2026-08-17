using FluentValidation;

namespace FixNow.Application.Features.Reviews.Commands.RestoreReview;

public sealed class RestoreReviewCommandValidator
    : AbstractValidator<RestoreReviewCommand>
{
    public RestoreReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId)
            .NotEmpty()
            .WithErrorCode("Review.IdRequired");
    }
}
