using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetTechnicianAccountStatus;

public sealed class GetTechnicianAccountStatusQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IUserRepository userRepository)
    : IQueryHandler<GetTechnicianAccountStatusQuery, Result<TechnicianAccountStatusResponse>>
{
    public async Task<Result<TechnicianAccountStatusResponse>> Handle(
        GetTechnicianAccountStatusQuery query,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await technicianProfileRepository.GetByIdAsync(
            query.TechnicianProfileId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var user = await userRepository.GetByIdAsync(
            technicianProfile.UserId,
            cancellationToken);

        if (user is null)
        {
            return UserErrors.NotFound;
        }

        return technicianProfile.ToTechnicianAccountStatusResponse(user);
    }
}
