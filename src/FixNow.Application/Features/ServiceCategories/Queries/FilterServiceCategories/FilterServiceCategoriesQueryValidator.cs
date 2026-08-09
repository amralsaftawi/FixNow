using FluentValidation;

namespace FixNow.Application.Features.ServiceCategories.Queries.FilterServiceCategories;

public sealed class FilterServiceCategoriesQueryValidator
    : AbstractValidator<FilterServiceCategoriesQuery>
{
    public FilterServiceCategoriesQueryValidator()
    {
        ValidateSearch();
        ValidateMinPrice();
        ValidateMaxPrice();
        ValidatePriceRange();
        ValidatePageNumber();
        ValidatePageSize();
    }

    private void ValidateSearch()
    {
        RuleFor(x => x.Search)
            .MaximumLength(100)
            .WithErrorCode("ServiceCategory.Search.TooLong");
    }

    private void ValidateMinPrice()
    {
        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("ServiceCategory.MinPrice.Invalid");
    }

    private void ValidateMaxPrice()
    {
        RuleFor(x => x.MaxPrice)
            .GreaterThan(0)
            .WithErrorCode("ServiceCategory.MaxPrice.Invalid");
    }

    private void ValidatePriceRange()
    {
        RuleFor(x => x.MinPrice)
            .Must((query, minPrice) =>
                !minPrice.HasValue
                || !query.MaxPrice.HasValue
                || minPrice <= query.MaxPrice)
            .WithErrorCode("ServiceCategory.PriceRange.Invalid");
    }

    private void ValidatePageNumber()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");
    }

    private void ValidatePageSize()
    {
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}
