using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.Identity.Commands.DeactivateCurrentUser;

public sealed class DeactivateCurrentUserCommandHandler(
    ICurrentUser currentUser,
    IUserRepository userRepository,
    global::IUnitOfWork unitOfWork)
    : ICommandHandler<DeactivateCurrentUserCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        DeactivateCurrentUserCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Ensure the user is authenticated.
        if (!currentUser.IsAuthenticated)
            return IdentityErrors.Unauthorized;

        // 2. Find the current user.
        var user = await userRepository.GetByIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (user is null)
            return UserErrors.NotFound;

        // 3. Deactivate the account.
        var deactivateResult = user.Deactivate();

        if (deactivateResult.IsError)
            return deactivateResult.Errors;

        // 4. Update the user.
        userRepository.Update(user);

        // 5. Persist changes.
        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        // 6. Return success.
        return Result.Success;
    }
}
