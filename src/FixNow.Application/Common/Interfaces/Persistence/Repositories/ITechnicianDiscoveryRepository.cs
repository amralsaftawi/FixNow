using FixNow.Application.Common.Models;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByLocation;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByRating;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByService;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianServices;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface ITechnicianDiscoveryRepository
{
    Task<PagedResult<NearbyTechnicianDto>> FindNearbyAsync(
        decimal latitude,
        decimal longitude,
        double radiusInKm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<PagedResult<ServiceTechnicianDto>> GetByServiceCategoryAsync(
        Guid serviceCategoryId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<PagedResult<LocatedTechnicianDto>> GetByCityAsync(
        int cityId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<PagedResult<RatedTechnicianDto>> GetByMinimumRatingAsync(
        double minimumRating,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TechnicianServiceDto>?> GetServicesByTechnicianAsync(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TechnicianServicePricingResponse>?> GetPricingByTechnicianAsync(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default);

    Task<TechnicianAvailabilitySettingsResponse?> GetAvailabilityByTechnicianAsync(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TechnicianPortfolioItemResponse>?> GetPortfolioByTechnicianAsync(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default);

    Task<TechnicianVerificationStatusResponse?> GetVerificationStatusByTechnicianAsync(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default);
}
