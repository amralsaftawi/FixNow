using FluentValidation;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetActiveServiceCategories;

public sealed class GetActiveServiceCategoriesQueryValidator
    : AbstractValidator<GetActiveServiceCategoriesQuery>
{
    public GetActiveServiceCategoriesQueryValidator()
    {
        ValidateSearch();

        ValidatePageNumber();

        ValidatePageSize();
    }

    private void ValidateSearch()
    {
        RuleFor(x => x.Search)
            .MaximumLength(100)
            .WithErrorCode("ServiceCategory.Search.TooLong");
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