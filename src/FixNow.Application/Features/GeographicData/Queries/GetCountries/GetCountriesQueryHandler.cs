using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.GeographicData.Dtos.Responses;
using FixNow.Application.Features.GeographicData.Mappers;

namespace FixNow.Application.Features.GeographicData.Queries.GetCountries;

public sealed class GetCountriesQueryHandler(
    ICountryRepository countryRepository)
    : IQueryHandler<GetCountriesQuery, Result<List<CountryResponse>>>
{
    public async Task<Result<List<CountryResponse>>> Handle(
        GetCountriesQuery query,
        CancellationToken cancellationToken)
    {
        var countries = await countryRepository.GetAllAsync(
            cancellationToken);

        return countries.ToResponses();
    }
}
