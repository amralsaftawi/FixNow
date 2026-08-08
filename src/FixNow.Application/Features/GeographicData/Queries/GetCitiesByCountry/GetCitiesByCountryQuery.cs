using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.GeographicData.Dtos.Responses;

namespace FixNow.Application.Features.GeographicData.Queries.GetCitiesByCountry;

public sealed record GetCitiesByCountryQuery(
    int CountryId)
    : IQuery<Result<List<CityResponse>>>;
