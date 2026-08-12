using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianAvailability;

public sealed class GetMyTechnicianAvailabilityQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : IQueryHandler<
        GetMyTechnicianAvailabilityQuery,
        Result<TechnicianAvailabilitySettingsResponse>>
{
    public async Task<Result<TechnicianAvailabilitySettingsResponse>> Handle(
        GetMyTechnicianAvailabilityQuery query,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return technicianProfile.ToTechnicianAvailabilitySettingsResponse();
    }
}
