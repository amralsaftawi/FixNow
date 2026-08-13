using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByLocation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByLocation;

public sealed class FilterTechniciansByLocationQueryHandler(
    ICityRepository cityRepository,
    ITechnicianDiscoveryRepository technicianDiscoveryRepository)
    : IQueryHandler<
        FilterTechniciansByLocationQuery,
        Result<FilterTechniciansByLocationResponse>>
{
    private readonly ICityRepository _cityRepository =
        cityRepository;

    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    public async Task<Result<FilterTechniciansByLocationResponse>> Handle(
        FilterTechniciansByLocationQuery query,
        CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(
            query.CityId,
            cancellationToken);

        if (city is null)
        {
            return CityErrors.NotFound;
        }

        var result = await _technicianDiscoveryRepository.GetByCityAsync(
            cityId: query.CityId,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken);

        return new FilterTechniciansByLocationResponse(
            Items: result.Items,
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}
