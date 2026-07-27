using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianProfile;

public sealed class UpdateTechnicianProfileCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateTechnicianProfileCommand, Result<Updated>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly ICurrentUser _currentUser =
        currentUser;

    public async Task<Result<Updated>> Handle(
        UpdateTechnicianProfileCommand command,
        CancellationToken cancellationToken)
    {
        var technicianProfile =
            await _technicianProfileRepository.GetByUserIdAsync(
                _currentUser.UserId,
                cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var yearsOfExperienceResult =
            technicianProfile.UpdateYearsOfExperience(
                command.YearsOfExperience);

        if (yearsOfExperienceResult.IsError)
        {
            return yearsOfExperienceResult.Errors;
        }

        var bioResult =technicianProfile.UpdateBio(command.Bio);

        if (bioResult.IsError)
        {
            return bioResult.Errors;
        }

        if (command.NationalIdImageKey is not null)
        {
            var nationalIdResult =
                technicianProfile.UpdateNationalId(
                    command.NationalIdImageKey);

            if (nationalIdResult.IsError)
            {
                return nationalIdResult.Errors;
            }
        }

        _technicianProfileRepository.Update(technicianProfile);

        return Result.Updated;
    }
}