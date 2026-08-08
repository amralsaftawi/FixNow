using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.GeographicData.Dtos.Responses;
using FixNow.Application.Features.GeographicData.Mappers;

namespace FixNow.Application.Features.GeographicData.Queries.GetCitiesByCountry;

public sealed class GetCitiesByCountryQueryHandler(
    ICityRepository cityRepository,
    ICountryRepository countryRepository)
    : IQueryHandler<GetCitiesByCountryQuery, Result<List<CityResponse>>>
{
    public async Task<Result<List<CityResponse>>> Handle(
        GetCitiesByCountryQuery query,
        CancellationToken cancellationToken)
    {
        if (!await countryRepository.ExistsByIdAsync(
                query.CountryId,
                cancellationToken))
        {
            return CountryErrors.NotFound;
        }

        var cities = await cityRepository.GetByCountryIdAsync(
            query.CountryId,
            cancellationToken);

        return cities.ToResponses();
    }
}
