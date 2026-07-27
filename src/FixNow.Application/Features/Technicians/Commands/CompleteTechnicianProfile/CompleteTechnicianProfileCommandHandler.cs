using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.CompleteTechnicianProfile;

public sealed class CompleteTechnicianProfileCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<CompleteTechnicianProfileCommand, Result<Updated>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly ICurrentUser _currentUser =
        currentUser;

    public async Task<Result<Updated>> Handle(
        CompleteTechnicianProfileCommand command,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await _technicianProfileRepository.GetByUserIdAsync(
            _currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var completionResult = technicianProfile.CompleteProfile();

        if (completionResult.IsError)
        {
            return completionResult.Errors;
        }

        _technicianProfileRepository.Update(technicianProfile);

        return Result.Updated;
    }
}