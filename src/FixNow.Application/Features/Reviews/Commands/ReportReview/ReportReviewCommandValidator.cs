using FluentValidation;

namespace FixNow.Application.Features.Reviews.Commands.ReportReview;

public sealed class ReportReviewCommandValidator
    : AbstractValidator<ReportReviewCommand>
{
    public ReportReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId)
            .NotEmpty()
            .WithErrorCode("Review.IdRequired");

        RuleFor(x => x.Reason)
            .IsInEnum()
            .WithErrorCode("ReviewReport.ReasonInvalid");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithErrorCode("ReviewReport.DescriptionTooLong");
    }
}
