using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByService;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByService;

public sealed class FilterTechniciansByServiceQueryHandler(
    IServiceCategoryRepository serviceCategoryRepository,
    ITechnicianDiscoveryRepository technicianDiscoveryRepository)
    : IQueryHandler<
        FilterTechniciansByServiceQuery,
        Result<FilterTechniciansByServiceResponse>>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    public async Task<Result<FilterTechniciansByServiceResponse>> Handle(
        FilterTechniciansByServiceQuery query,
        CancellationToken cancellationToken)
    {
        var serviceCategory = await _serviceCategoryRepository.GetByIdAsync(
            query.ServiceCategoryId,
            cancellationToken);

        if (serviceCategory is null || !serviceCategory.IsActive)
        {
            return ServiceCategoryErrors.NotFound;
        }

        var result = await _technicianDiscoveryRepository.GetByServiceCategoryAsync(
            serviceCategoryId: query.ServiceCategoryId,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken);

        return new FilterTechniciansByServiceResponse(
            Items: result.Items,
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}
