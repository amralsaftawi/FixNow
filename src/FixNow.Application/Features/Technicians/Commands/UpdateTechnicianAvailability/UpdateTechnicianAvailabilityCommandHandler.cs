using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAvailability;

public sealed class UpdateTechnicianAvailabilityCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<
        UpdateTechnicianAvailabilityCommand,
        Result<TechnicianAvailabilityResponse>>
{
    public async Task<Result<TechnicianAvailabilityResponse>> Handle(
        UpdateTechnicianAvailabilityCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate, and it guarantees
        //    the availability being modified is always the caller's own.
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. Apply the availability state through the aggregate so the
        //    domain invariants (e.g. no-op when unchanged) and the domain
        //    event are handled by the domain model.
        var updateResult = technicianProfile.UpdateAvailability(
            command.Availability);

        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        // 3. Persist through the existing repository/unit-of-work pipeline.
        technicianProfileRepository.Update(technicianProfile);

        return technicianProfile.ToTechnicianAvailabilityResponse();
    }
}
