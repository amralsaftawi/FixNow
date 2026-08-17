using FluentValidation;

namespace FixNow.Application.Features.Reviews.Queries.GetTechnicianReviews;

public sealed class GetTechnicianReviewsQueryValidator
    : AbstractValidator<GetTechnicianReviewsQuery>
{
    public GetTechnicianReviewsQueryValidator()
    {
        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("Review.TechnicianProfile.Required");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}
