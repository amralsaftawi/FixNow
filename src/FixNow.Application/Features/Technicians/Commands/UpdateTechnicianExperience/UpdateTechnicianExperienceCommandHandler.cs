using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianExperience;

public sealed class UpdateTechnicianExperienceCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateTechnicianExperienceCommand, Result<TechnicianExperienceResponse>>
{
    public async Task<Result<TechnicianExperienceResponse>> Handle(
        UpdateTechnicianExperienceCommand command,
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

        // 2. Find the experience within the current user's profile.
        var experience = technicianProfile.Experiences
            .FirstOrDefault(item => item.Id == command.ExperienceId);

        if (experience is null)
        {
            return TechnicianProfileErrors.ExperienceNotFound;
        }

        // 3. Update the experience.
        var updateResult = experience.Update(
            companyName: command.CompanyName,
            position: command.Position,
            description: command.Description,
            startDate: command.StartDate,
            endDate: command.EndDate);

        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        // 4. Return the updated experience.
        return experience.ToTechnicianExperienceResponse();
    }
}
