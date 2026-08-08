using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.Identity.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    IOtpRepository otpRepository,
    IPasswordHasher passwordHasher,
    global::IUnitOfWork unitOfWork)
    : ICommandHandler<
        ResetPasswordCommand,
        Result<ResetPasswordResponse>>
{
    private const string SuccessMessage =
        "Password has been reset successfully.";

    public async Task<Result<ResetPasswordResponse>> Handle(
        ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Find user by email or phone.
        var user = await FindUserAsync(
            command.Identifier,
            cancellationToken);

        if (user is null)
            return IdentityErrors.InvalidResetPasswordRequest;

        // 2. Get latest password-reset OTP.
        var otpRecord = await otpRepository.GetLatestAsync(
            user.Id,
            OtpPurpose.PasswordReset,
            cancellationToken);

        if (otpRecord is null)
            return OTPRecordErrors.InvalidOtp;

        // 3. Validate OTP state.
        if (otpRecord.IsExpired)
            return OTPRecordErrors.Expired;

        if (otpRecord.IsInvalidated)
            return OTPRecordErrors.AlreadyInvalidated;

        if (!otpRecord.IsVerified)
            return OTPRecordErrors.InvalidOtp;

        // 4. Hash the new password.

        var hashedPassword = passwordHasher.Hash(command.NewPassword);

var passwordHashResult = PasswordHash.Create(hashedPassword);

if (passwordHashResult.IsError)
    return passwordHashResult.Errors;

var changePasswordResult = user.ChangePassword(
    passwordHashResult.Value);

        // 5. Change user's password.


        if (changePasswordResult.IsError)
            return changePasswordResult.Errors;

        // 6. Invalidate the verified OTP.
        var invalidateResult = otpRecord.Invalidate();

        if (invalidateResult.IsError)
            return invalidateResult.Errors;

        // 7. Update entities.
        userRepository.Update(user);

        await otpRepository.UpdateAsync(
            otpRecord,
            cancellationToken);

        // 8. Persist changes.
        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        // 9. Return success.
        return new ResetPasswordResponse(
            SuccessMessage);
    }

    private async Task<User?> FindUserAsync(
        string identifier,
        CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(identifier);

        if (!emailResult.IsError)
        {
            return await userRepository.GetByEmailAsync(
                emailResult.Value,
                cancellationToken);
        }

        var phoneResult = PhoneNumber.Create(identifier);

        if (!phoneResult.IsError)
        {
            return await userRepository.GetByPhoneNumberAsync(
                phoneResult.Value,
                cancellationToken);
        }

        return null;
    }
}