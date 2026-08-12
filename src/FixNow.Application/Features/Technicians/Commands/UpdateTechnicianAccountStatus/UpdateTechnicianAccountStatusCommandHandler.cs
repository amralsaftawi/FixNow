using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAccountStatus;

public sealed class UpdateTechnicianAccountStatusCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IUserRepository userRepository)
    : ICommandHandler<UpdateTechnicianAccountStatusCommand, Result<TechnicianAccountStatusResponse>>
{
    public async Task<Result<TechnicianAccountStatusResponse>> Handle(
        UpdateTechnicianAccountStatusCommand command,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await technicianProfileRepository.GetByIdAsync(
            command.TechnicianProfileId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var user = await userRepository.GetByIdAsync(
            technicianProfile.UserId,
            cancellationToken);

        if (user is null)
        {
            return UserErrors.NotFound;
        }

        var result = ApplyStatus(user, command.Status);

        if (result.IsError)
        {
            return result.Errors;
        }

        userRepository.Update(user);

        return technicianProfile.ToTechnicianAccountStatusResponse(user);
    }

    private static Result<Success> ApplyStatus(User user, AccountStatus status)
        => status switch
        {
            AccountStatus.Active => user.Activate(),
            AccountStatus.Suspended => user.Suspend(),
            AccountStatus.Deactivated => user.Deactivate(),
            _ => UserErrors.InvalidAccountStatus,
        };
}
