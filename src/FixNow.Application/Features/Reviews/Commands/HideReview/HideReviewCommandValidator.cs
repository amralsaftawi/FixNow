using FluentValidation;

namespace FixNow.Application.Features.Reviews.Commands.HideReview;

public sealed class HideReviewCommandValidator
    : AbstractValidator<HideReviewCommand>
{
    public HideReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId)
            .NotEmpty()
            .WithErrorCode("Review.IdRequired");
    }
}
