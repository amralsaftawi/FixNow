using FluentValidation;

namespace FixNow.Application.Features.CustomerRatings.Queries.GetCustomerRating;

public sealed class GetCustomerRatingQueryValidator
    : AbstractValidator<GetCustomerRatingQuery>
{
    public GetCustomerRatingQueryValidator()
    {
        RuleFor(x => x.CustomerProfileId)
            .NotEmpty()
            .WithErrorCode("CustomerRating.CustomerProfile.Required");
    }
}
