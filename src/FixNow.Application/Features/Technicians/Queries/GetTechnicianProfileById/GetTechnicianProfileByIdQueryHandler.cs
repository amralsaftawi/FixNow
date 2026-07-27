using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetTechnicianProfileById;

public sealed class GetTechnicianProfileByIdQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository)
    : IQueryHandler<
        GetTechnicianProfileByIdQuery,
        Result<TechnicianProfileResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    public async Task<Result<TechnicianProfileResponse>> Handle(
        GetTechnicianProfileByIdQuery query,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await _technicianProfileRepository.GetByIdAsync(
            query.TechnicianProfileId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return technicianProfile.ToTechnicianProfileResponse();
    }
}