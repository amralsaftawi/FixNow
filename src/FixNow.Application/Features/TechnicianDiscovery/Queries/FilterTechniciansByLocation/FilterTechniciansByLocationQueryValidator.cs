using FluentValidation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByLocation;

public sealed class FilterTechniciansByLocationQueryValidator
    : AbstractValidator<FilterTechniciansByLocationQuery>
{
    public FilterTechniciansByLocationQueryValidator()
    {
        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .WithErrorCode("TechnicianDiscovery.CityId.Invalid");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}
