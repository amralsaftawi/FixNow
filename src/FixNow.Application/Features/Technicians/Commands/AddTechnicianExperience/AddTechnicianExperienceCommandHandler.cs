using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.AddTechnicianExperience;

public sealed class AddTechnicianExperienceCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<AddTechnicianExperienceCommand, Result<TechnicianExperienceResponse>>
{
    public async Task<Result<TechnicianExperienceResponse>> Handle(
        AddTechnicianExperienceCommand command,
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

        // 2. Create the experience.
        var experienceResult = TechnicianExperience.Create(
            id: Guid.NewGuid(),
            technicianProfileId: technicianProfile.Id,
            companyName: command.CompanyName,
            position: command.Position,
            description: command.Description,
            startDate: command.StartDate,
            endDate: command.EndDate);

        if (experienceResult.IsError)
        {
            return experienceResult.Errors;
        }

        // 3. Add the experience to the profile.
        var addResult = technicianProfile.AddExperience(
            experienceResult.Value);

        if (addResult.IsError)
        {
            return addResult.Errors;
        }

        // 4. Track the new experience so it is inserted.
        await technicianProfileRepository.AddExperienceAsync(
            experienceResult.Value,
            cancellationToken);

        // 5. Return the created experience.
        return experienceResult.Value.ToTechnicianExperienceResponse();
    }
}
