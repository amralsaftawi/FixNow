using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianJobHistory;

public sealed class GetTechnicianJobHistoryQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IServiceRequestRepository serviceRequestRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetTechnicianJobHistoryQuery, Result<GetTechnicianJobHistoryResponse>>
{
    public async Task<Result<GetTechnicianJobHistoryResponse>> Handle(
        GetTechnicianJobHistoryQuery query,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate: a user without a
        //    technician profile cannot access technician request management.
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. Query the technician's historical jobs. The technician data
        //    scope is applied at the database level (see repository), so a
        //    technician can only ever see their own history.
        var result = await serviceRequestRepository.GetJobHistoryForTechnicianAsync(
            technicianProfile.Id,
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        return new GetTechnicianJobHistoryResponse(
            Items: result.Items,
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}
