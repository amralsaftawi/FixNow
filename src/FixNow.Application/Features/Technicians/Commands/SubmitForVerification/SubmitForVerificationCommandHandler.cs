using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Authentication;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.SubmitForVerification;

public sealed class SubmitForVerificationCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<SubmitForVerificationCommand, Result<TechnicianProfileResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<TechnicianProfileResponse>> Handle(
        SubmitForVerificationCommand command,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await _technicianProfileRepository.GetByUserIdAsync(
            _currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var submitResult = technicianProfile.SubmitForVerification();

        if (submitResult.IsError)
        {
            return submitResult.Errors;
        }

        _technicianProfileRepository.Update(technicianProfile);

        return technicianProfile.ToTechnicianProfileResponse();
    }
}
