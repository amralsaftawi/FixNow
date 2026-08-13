using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianRequests.Queries.GetAvailableServiceRequests;

public sealed class GetAvailableServiceRequestsQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IServiceRequestRepository serviceRequestRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetAvailableServiceRequestsQuery, Result<GetAvailableServiceRequestsResponse>>
{
    public async Task<Result<GetAvailableServiceRequestsResponse>> Handle(
        GetAvailableServiceRequestsQuery query,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate: a user without a
        //    technician profile cannot access technician request management.
        var technicianProfile = await technicianProfileRepository.GetByUserIdWithServicesAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. The technician is only eligible for requests in the service
        //    categories they have selected.
        var serviceCategoryIds = technicianProfile.Services
            .Select(service => service.ServiceCategoryId)
            .ToList();

        // 3. Query only requests that are currently searching for a
        //    technician and match one of the technician's categories.
        //    Filtering happens at the database level.
        var result = await serviceRequestRepository.GetAvailableForTechnicianAsync(
            serviceCategoryIds,
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        return new GetAvailableServiceRequestsResponse(
            Items: result.Items,
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}
