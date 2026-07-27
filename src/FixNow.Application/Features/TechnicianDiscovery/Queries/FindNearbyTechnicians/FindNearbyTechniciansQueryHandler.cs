using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianDiscovery.Mappers;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians;

public sealed class FindNearbyTechniciansQueryHandler(
    ITechnicianDiscoveryRepository technicianDiscoveryRepository,
    IServiceCategoryRepository serviceCategoryRepository)
    : IQueryHandler<FindNearbyTechniciansQuery, Result<FindNearbyTechniciansResponse>>
{
    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    public async Task<Result<FindNearbyTechniciansResponse>> Handle(
        FindNearbyTechniciansQuery query,
        CancellationToken cancellationToken)
    {
        var serviceCategory = await _serviceCategoryRepository.GetByIdAsync(
            query.ServiceCategoryId,
            cancellationToken);

        if (serviceCategory is null || !serviceCategory.IsActive)
        {
            return ServiceCategoryErrors.NotFound;
        }

        var result = await _technicianDiscoveryRepository.FindNearbyAsync(
            serviceCategoryId: query.ServiceCategoryId,
            latitude: query.Latitude,
            longitude: query.Longitude,
            radiusInKm: query.RadiusInKm,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken);

        return new FindNearbyTechniciansResponse(
            Items: result.Items.ToNearbyDtos(),
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}