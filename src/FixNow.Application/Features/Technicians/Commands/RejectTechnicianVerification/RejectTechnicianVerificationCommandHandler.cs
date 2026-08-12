using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RejectTechnicianVerification;

public sealed class RejectTechnicianVerificationCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository)
    : ICommandHandler<RejectTechnicianVerificationCommand, Result<TechnicianProfileResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    public async Task<Result<TechnicianProfileResponse>> Handle(
        RejectTechnicianVerificationCommand command,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await _technicianProfileRepository.GetByIdAsync(
            command.TechnicianProfileId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var rejectResult = technicianProfile.RejectVerification();

        if (rejectResult.IsError)
        {
            return rejectResult.Errors;
        }

        _technicianProfileRepository.Update(technicianProfile);

        return technicianProfile.ToTechnicianProfileResponse();
    }
}
