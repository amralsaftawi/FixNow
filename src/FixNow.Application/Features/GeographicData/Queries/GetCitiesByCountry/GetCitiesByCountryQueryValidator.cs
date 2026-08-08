using FluentValidation;

namespace FixNow.Application.Features.GeographicData.Queries.GetCitiesByCountry;

public sealed class GetCitiesByCountryQueryValidator
    : AbstractValidator<GetCitiesByCountryQuery>
{
    public GetCitiesByCountryQueryValidator()
    {
        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithErrorCode("Country.IdRequired");
    }
}
