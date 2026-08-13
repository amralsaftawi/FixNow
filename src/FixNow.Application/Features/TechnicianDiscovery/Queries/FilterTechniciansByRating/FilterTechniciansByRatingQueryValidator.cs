using FluentValidation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByRating;

public sealed class FilterTechniciansByRatingQueryValidator
    : AbstractValidator<FilterTechniciansByRatingQuery>
{
    public FilterTechniciansByRatingQueryValidator()
    {
        RuleFor(x => x.MinimumRating)
            .InclusiveBetween(1, 5)
            .WithErrorCode("TechnicianDiscovery.Rating.Invalid");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}
