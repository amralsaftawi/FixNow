using FluentValidation;

namespace FixNow.Application.Features.Reviews.Commands.CreateReview;

public sealed class CreateReviewCommandValidator
    : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithErrorCode("Review.CommentRequired")
            .MaximumLength(1000)
            .WithErrorCode("Review.CommentTooLong");
    }
}
