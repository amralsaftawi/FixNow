using FluentValidation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByService;

public sealed class FilterTechniciansByServiceQueryValidator
    : AbstractValidator<FilterTechniciansByServiceQuery>
{
    public FilterTechniciansByServiceQueryValidator()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .WithErrorCode("TechnicianDiscovery.ServiceCategory.Required");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}
