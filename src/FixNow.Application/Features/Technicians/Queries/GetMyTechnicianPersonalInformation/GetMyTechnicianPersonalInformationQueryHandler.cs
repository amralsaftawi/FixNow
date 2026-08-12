using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Authentication;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianPersonalInformation;

public sealed class GetMyTechnicianPersonalInformationQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IUserRepository userRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetMyTechnicianPersonalInformationQuery, Result<TechnicianPersonalInformationResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly IUserRepository _userRepository = userRepository;

    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<TechnicianPersonalInformationResponse>> Handle(
        GetMyTechnicianPersonalInformationQuery query,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var technicianProfile = await _technicianProfileRepository.GetByUserIdAsync(
            userId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var user = await _userRepository.GetByIdAsync(
            userId,
            cancellationToken);

        if (user is null)
        {
            return UserErrors.NotFound;
        }

        return user.ToTechnicianPersonalInformationResponse(technicianProfile);
    }
}
