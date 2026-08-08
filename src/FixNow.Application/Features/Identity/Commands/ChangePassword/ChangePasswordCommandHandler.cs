using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.Identity.Commands.ChangePassword;

public sealed class ChangePasswordCommandHandler(
    ICurrentUser currentUser,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    global::IUnitOfWork unitOfWork)
    : ICommandHandler<ChangePasswordCommand, Result<ChangePasswordResponse>>
{
    private const string SuccessMessage =
        "Password has been changed successfully.";

    public async Task<Result<ChangePasswordResponse>> Handle(
        ChangePasswordCommand command,
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

        // 3. Verify the current password.
        var isCurrentPasswordValid = passwordHasher.Verify(
            command.CurrentPassword,
            user.PasswordHash);

        if (!isCurrentPasswordValid)
            return IdentityErrors.IncorrectPassword;

        // 4. Hash the new password.
        var hashedPassword = passwordHasher.Hash(
            command.NewPassword);

        var passwordHashResult = PasswordHash.Create(
            hashedPassword);

        if (passwordHashResult.IsError)
            return passwordHashResult.Errors;

        // 5. Change the user's password.
        var changePasswordResult = user.ChangePassword(
            passwordHashResult.Value);

        if (changePasswordResult.IsError)
            return changePasswordResult.Errors;

        // 6. Update the user.
        userRepository.Update(user);

        // 7. Persist changes.
        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        // 8. Return success.
        return new ChangePasswordResponse(
            SuccessMessage);
    }
}
