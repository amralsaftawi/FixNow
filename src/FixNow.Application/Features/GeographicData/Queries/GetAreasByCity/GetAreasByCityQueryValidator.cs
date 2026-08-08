using FluentValidation;

namespace FixNow.Application.Features.GeographicData.Queries.GetAreasByCity;

public sealed class GetAreasByCityQueryValidator
    : AbstractValidator<GetAreasByCityQuery>
{
    public GetAreasByCityQueryValidator()
    {
        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .WithErrorCode("City.IdRequired");
    }
}
