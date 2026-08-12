using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Commands.RegisterTechnician;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianProfile;

public sealed class UpdateTechnicianProfileCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateTechnicianProfileCommand, Result<TechnicianProfileResponse>>
{
    public async Task<Result<TechnicianProfileResponse>> Handle(
        UpdateTechnicianProfileCommand command,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var bio = command.Bio?.Trim();

        var nationalIdImageKey = command.NationalIdImageKey?.Trim();

        // Validate the national ID image before mutating anything so a failed
        // validation never leaves the profile partially updated.
        if (technicianProfile.NationalIdImageKey != nationalIdImageKey)
        {
            if (!string.IsNullOrWhiteSpace(nationalIdImageKey)
                && !IsOwnedByCurrentUser(
                    nationalIdImageKey,
                    currentUser.UserId))
            {
                return TechnicianProfileErrors.NationalIdImageOwnershipInvalid;
            }

            if (string.IsNullOrWhiteSpace(nationalIdImageKey))
            {
                return TechnicianProfileErrors.NationalIdImageRequired;
            }
        }

        if (technicianProfile.YearsOfExperience != command.YearsOfExperience)
        {
            var yearsOfExperienceResult = technicianProfile.UpdateYearsOfExperience(
                command.YearsOfExperience);

            if (yearsOfExperienceResult.IsError)
            {
                return yearsOfExperienceResult.Errors;
            }
        }

        if (technicianProfile.Bio != bio)
        {
            var bioResult = technicianProfile.UpdateBio(bio);

            if (bioResult.IsError)
            {
                return bioResult.Errors;
            }
        }

        if (technicianProfile.NationalIdImageKey != nationalIdImageKey)
        {
            var nationalIdResult = technicianProfile.UpdateNationalId(
                nationalIdImageKey);

            if (nationalIdResult.IsError)
            {
                return nationalIdResult.Errors;
            }
        }

        technicianProfileRepository.Update(technicianProfile);

        return technicianProfile.ToTechnicianProfileResponse();
    }

    private static bool IsOwnedByCurrentUser(
        string nationalIdImageKey,
        Guid userId)
    {
        var expectedPrefix =
            $"{RegisterTechnicianCommand.NationalIdFolderPrefix}/{userId}/";

        return nationalIdImageKey.StartsWith(
            expectedPrefix,
            StringComparison.OrdinalIgnoreCase);
    }
}
