using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAvailability;

public sealed class UpdateTechnicianAvailabilityCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateTechnicianAvailabilityCommand, Result<Updated>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly ICurrentUser _currentUser =
        currentUser;

    public async Task<Result<Updated>> Handle(
        UpdateTechnicianAvailabilityCommand command,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await _technicianProfileRepository.GetByUserIdAsync(
            _currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var availabilityResult = technicianProfile.UpdateAvailability(
            command.Availability);

        if (availabilityResult.IsError)
        {
            return availabilityResult.Errors;
        }

        _technicianProfileRepository.Update(technicianProfile);

        return Result.Updated;
    }
}