using FluentValidation;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceCategories;

public sealed class GetServiceCategoriesQueryValidator
    : AbstractValidator<GetServiceCategoriesQuery>
{
    public GetServiceCategoriesQueryValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(100)
            .WithErrorCode("ServiceCategory.Search.TooLong");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}