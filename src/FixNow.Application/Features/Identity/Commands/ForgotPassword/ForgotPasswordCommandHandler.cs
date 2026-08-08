using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Authentication;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Identity.Commands.ForgotPassword;

public sealed class ForgotPasswordCommandHandler(
    IUserRepository userRepository,
    IOtpRepository otpRepository,
    IOtpGenerator otpGenerator,
    IOtpHasher otpHasher,
    IOtpSender otpSender,
    global::IUnitOfWork unitOfWork)
    : ICommandHandler<ForgotPasswordCommand, Result<ForgotPasswordResponse>>
{
    private const string SuccessMessage =
        "If the account exists, password reset instructions have been sent.";

    private const int OtpExpirationMinutes = 5;
    private const int OtpMaxAttempts = 5;

    public async Task<Result<ForgotPasswordResponse>> Handle(
        ForgotPasswordCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Find user by email or phone.
        var user = await FindUserAsync(
            command.Identifier,
            cancellationToken);

        // 2. Do not reveal whether the account exists.
        if (user is null)
            return CreateSuccessResponse();

        // 3. Generate OTP.
        var otpResult = otpGenerator.Generate();

        if (otpResult.IsError)
            return otpResult.Errors;

        var otp = otpResult.Value;

        // 4. Hash OTP before storing it.
        var codeHash = otpHasher.Hash(otp.Code);

        // 5. Create OTP record.
        var expiresAt = DateTimeOffset.UtcNow
            .AddMinutes(OtpExpirationMinutes);

        var otpRecordResult = OTPRecord.Create(
            id: Guid.NewGuid(),
            userId: user.Id,
            codeHash: codeHash,
            purpose: OtpPurpose.PasswordReset,
            expiresAt: expiresAt,
            maxAttempts: OtpMaxAttempts);

        if (otpRecordResult.IsError)
            return otpRecordResult.Errors;

        var otpRecord = otpRecordResult.Value;

        // 6. Store OTP record.
        await otpRepository.AddAsync(
            otpRecord,
            cancellationToken);

        // 7. Send OTP through the existing abstraction.
        var sendResult = await otpSender.SendAsync(
            user,
            otp.Code,
            OtpPurpose.PasswordReset,
            cancellationToken);

        if (sendResult.IsError)
            return sendResult.Errors;

        // 8. Persist everything.
        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        // 9. Return generic success response.
        return CreateSuccessResponse();
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

    private static ForgotPasswordResponse CreateSuccessResponse()
        => new(SuccessMessage);
}