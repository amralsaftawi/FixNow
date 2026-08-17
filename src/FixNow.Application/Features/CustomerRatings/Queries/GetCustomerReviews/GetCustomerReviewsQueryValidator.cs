using FluentValidation;

namespace FixNow.Application.Features.CustomerRatings.Queries.GetCustomerReviews;

public sealed class GetCustomerReviewsQueryValidator
    : AbstractValidator<GetCustomerReviewsQuery>
{
    public GetCustomerReviewsQueryValidator()
    {
        RuleFor(x => x.CustomerProfileId)
            .NotEmpty()
            .WithErrorCode("CustomerRating.CustomerProfile.Required");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}
