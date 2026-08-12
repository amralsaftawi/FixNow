using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianExperiences;

public sealed class GetMyTechnicianExperiencesQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetMyTechnicianExperiencesQuery, Result<List<TechnicianExperienceResponse>>>
{
    public async Task<Result<List<TechnicianExperienceResponse>>> Handle(
        GetMyTechnicianExperiencesQuery query,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return technicianProfile.Experiences
            .OrderByDescending(experience => experience.StartDate)
            .ToDtos();
    }
}
