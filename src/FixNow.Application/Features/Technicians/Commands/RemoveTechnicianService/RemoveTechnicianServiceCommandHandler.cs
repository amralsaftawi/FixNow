using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RemoveTechnicianService;

public sealed class RemoveTechnicianServiceCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<RemoveTechnicianServiceCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        RemoveTechnicianServiceCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Find the current user's technician profile (with services loaded).
        var technicianProfile = await technicianProfileRepository
            .GetByUserIdWithServicesAsync(
                currentUser.UserId,
                cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. Find the service to remove (one service per category per profile).
        var service = technicianProfile.Services
            .FirstOrDefault(x => x.ServiceCategoryId == command.ServiceCategoryId);

        if (service is null)
        {
            return TechnicianProfileErrors.ServiceNotFound;
        }

        // 3. Remove the service from the profile.
        var removeResult = technicianProfile.RemoveService(
            service.Id);

        if (removeResult.IsError)
        {
            return removeResult.Errors;
        }

        // 4. Track the service for deletion.
        technicianProfileRepository.RemoveService(service);

        return Result.Success;
    }
}
