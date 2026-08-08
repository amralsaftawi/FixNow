using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.GeographicData.Dtos.Responses;
using FixNow.Application.Features.GeographicData.Mappers;

namespace FixNow.Application.Features.GeographicData.Queries.GetAreasByCity;

public sealed class GetAreasByCityQueryHandler(
    IAreaRepository areaRepository,
    ICityRepository cityRepository)
    : IQueryHandler<GetAreasByCityQuery, Result<List<AreaResponse>>>
{
    public async Task<Result<List<AreaResponse>>> Handle(
        GetAreasByCityQuery query,
        CancellationToken cancellationToken)
    {
        if (!await cityRepository.ExistsByIdAsync(
                query.CityId,
                cancellationToken))
        {
            return CityErrors.NotFound;
        }

        var areas = await areaRepository.GetByCityIdAsync(
            query.CityId,
            cancellationToken);

        return areas.ToResponses();
    }
}
