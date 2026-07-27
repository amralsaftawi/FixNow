using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianProfile;

public sealed class GetMyTechnicianProfileQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : IQueryHandler<
        GetMyTechnicianProfileQuery,
        Result<TechnicianProfileResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly ICurrentUser _currentUser =
        currentUser;

    public async Task<Result<TechnicianProfileResponse>> Handle(
        GetMyTechnicianProfileQuery query,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await _technicianProfileRepository.GetByUserIdAsync(
            _currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return technicianProfile.ToTechnicianProfileResponse();
    }
}