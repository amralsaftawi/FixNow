using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianRequests.Queries.GetServiceRequestDetails;

public sealed class GetServiceRequestDetailsQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IServiceRequestRepository serviceRequestRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetServiceRequestDetailsQuery, Result<GetServiceRequestDetailsResponse>>
{
    public async Task<Result<GetServiceRequestDetailsResponse>> Handle(
        GetServiceRequestDetailsQuery query,
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

        // 2. The technician is only allowed to view requests in the service
        //    categories they have selected. The data scope is applied inside
        //    the database query so an inaccessible or non-existent request
        //    both resolve to the same outcome without leaking information.
        var serviceCategoryIds = technicianProfile.Services
            .Select(service => service.ServiceCategoryId)
            .ToList();

        var details = await serviceRequestRepository.GetDetailsForTechnicianAsync(
            query.ServiceRequestId,
            serviceCategoryIds,
            cancellationToken);

        if (details is null)
        {
            return ServiceRequestErrors.NotFound;
        }

        return new GetServiceRequestDetailsResponse(details);
    }
}
