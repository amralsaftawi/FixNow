using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.GeographicData.Dtos.Responses;

namespace FixNow.Application.Features.GeographicData.Queries.GetCountries;

public sealed record GetCountriesQuery
    : IQuery<Result<List<CountryResponse>>>;
