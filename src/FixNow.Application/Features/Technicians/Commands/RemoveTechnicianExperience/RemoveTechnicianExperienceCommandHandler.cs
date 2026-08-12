using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RemoveTechnicianExperience;

public sealed class RemoveTechnicianExperienceCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<RemoveTechnicianExperienceCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        RemoveTechnicianExperienceCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Find the current user's technician profile.
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. Find the experience to remove.
        var experience = technicianProfile.Experiences
            .FirstOrDefault(x => x.Id == command.ExperienceId);

        if (experience is null)
        {
            return TechnicianProfileErrors.ExperienceNotFound;
        }

        // 3. Remove the experience from the profile.
        var removeResult = technicianProfile.RemoveExperience(
            command.ExperienceId);

        if (removeResult.IsError)
        {
            return removeResult.Errors;
        }

        // 4. Track the experience for deletion.
        technicianProfileRepository.RemoveExperience(experience);

        return Result.Success;
    }
}
