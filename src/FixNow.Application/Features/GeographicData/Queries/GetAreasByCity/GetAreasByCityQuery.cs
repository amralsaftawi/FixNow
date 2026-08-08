using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.GeographicData.Dtos.Responses;

namespace FixNow.Application.Features.GeographicData.Queries.GetAreasByCity;

public sealed record GetAreasByCityQuery(
    int CityId)
    : IQuery<Result<List<AreaResponse>>>;
