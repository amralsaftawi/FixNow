using FluentValidation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians;

public sealed class FindNearbyTechniciansQueryValidator
    : AbstractValidator<FindNearbyTechniciansQuery>
{
    public FindNearbyTechniciansQueryValidator()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .WithErrorCode("TechnicianDiscovery.ServiceCategoryId.Required");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .WithErrorCode("TechnicianDiscovery.Latitude.Invalid");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .WithErrorCode("TechnicianDiscovery.Longitude.Invalid");

        RuleFor(x => x.RadiusInKm)
            .GreaterThan(0)
            .LessThanOrEqualTo(20)
            .WithErrorCode("TechnicianDiscovery.Radius.Invalid");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Pagination.PageNumber.Invalid");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode("Pagination.PageSize.Invalid");
    }
}