using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.VerifyTechnician;

public sealed class VerifyTechnicianCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository)
    : ICommandHandler<VerifyTechnicianCommand, Result<TechnicianProfileResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    public async Task<Result<TechnicianProfileResponse>> Handle(
        VerifyTechnicianCommand command,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await _technicianProfileRepository.GetByIdAsync(
            command.TechnicianProfileId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var verifyResult = technicianProfile.Verify();

        if (verifyResult.IsError)
        {
            return verifyResult.Errors;
        }

        _technicianProfileRepository.Update(technicianProfile);

        return technicianProfile.ToTechnicianProfileResponse();
    }
}
