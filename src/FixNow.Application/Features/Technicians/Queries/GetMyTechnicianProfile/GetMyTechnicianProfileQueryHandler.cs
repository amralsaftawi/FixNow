using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianProfile;

public sealed class GetMyTechnicianProfileQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetMyTechnicianProfileQuery, Result<TechnicianProfileResponse>>
{
    public async Task<Result<TechnicianProfileResponse>> Handle(GetMyTechnicianProfileQuery query,CancellationToken cancellationToken)
    {
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(currentUser.UserId, cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return technicianProfile.ToTechnicianProfileResponse();
    }
}
