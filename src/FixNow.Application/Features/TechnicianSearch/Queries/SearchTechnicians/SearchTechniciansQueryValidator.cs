using FluentValidation;

namespace FixNow.Application.Features.TechnicianSearch.Queries.SearchTechnicians;

public sealed class SearchTechniciansQueryValidator
    : AbstractValidator<SearchTechniciansQuery>
{
    public SearchTechniciansQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
            .NotEmpty()
            .WithErrorCode("TechnicianSearch.Term.Required")
            .MaximumLength(100)
            .WithErrorCode("TechnicianSearch.Term.TooLong");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}
