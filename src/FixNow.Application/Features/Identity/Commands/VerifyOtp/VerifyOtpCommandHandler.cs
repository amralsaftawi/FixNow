using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.Identity.Commands.VerifyOtp.Processors;

namespace FixNow.Application.Features.Identity.Commands.VerifyOtp;

public sealed class VerifyOtpCommandHandler(
    IUserRepository userRepository,
    IOtpRepository otpRepository,
    IOtpHasher otpHasher,
    IEnumerable<IOtpPurposeProcessor> processors,
    global::IUnitOfWork unitOfWork)
    : ICommandHandler<VerifyOtpCommand, Result<VerifyOtpResponse>>
{
    private const string SuccessMessage = "OTP verified successfully.";

    public async Task<Result<VerifyOtpResponse>> Handle(
        VerifyOtpCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Find user by email or phone.
        var user = await FindUserAsync(command.Identifier, cancellationToken);

        // 2. If user not found -> Return success message (do not reveal whether account exists).
        if (user is null)
            return CreateSuccessResponse();

        if (!Enum.TryParse<OtpPurpose>(command.Purpose, ignoreCase: true, out var purpose))
            return OTPRecordErrors.InvalidPurpose;

        // 3. Get latest OTP for User and Purpose.
        var otpRecord = await otpRepository.GetLatestAsync(
            user.Id,
            purpose,
            cancellationToken);

        // 4. If no OTP exists -> Return InvalidOtp error.
        if (otpRecord is null)
            return OTPRecordErrors.InvalidOtp;

        // 5. If OTP is expired -> Return OtpExpired error.
        if (otpRecord.IsExpired)
            return OTPRecordErrors.OtpExpired;

        // 6. If OTP is already used -> Return OtpAlreadyUsed error.
        if (otpRecord.IsVerified || otpRecord.IsInvalidated)
            return OTPRecordErrors.OtpAlreadyUsed;

        // 7. Verify OTP hash.
        var isValid = otpHasher.Verify(command.Otp, otpRecord.CodeHash);

        // 8. If verification fails -> Return InvalidOtp error.
        if (!isValid)
            return OTPRecordErrors.InvalidOtp;

        // 9. Mark OTP as used.
        var verifyOtpResult = otpRecord.Verify();
        if (verifyOtpResult.IsError)
            return verifyOtpResult.Errors;

        await otpRepository.UpdateAsync(otpRecord, cancellationToken);

        // 10. Resolve processor & execute for Purpose.
        var processor = processors.FirstOrDefault(p => p.Purpose == purpose);
        if (processor is null)
            return OTPRecordErrors.InvalidPurpose;

        var processResult = await processor.ProcessAsync(user, cancellationToken);
        if (processResult.IsError)
            return processResult.Errors;

        // 11. Save changes through UnitOfWork.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 12. Return success response.
        return CreateSuccessResponse();
    }

    private async Task<User?> FindUserAsync(
        string identifier,
        CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(identifier);
        if (!emailResult.IsError)
        {
            return await userRepository.GetByEmailAsync(emailResult.Value, cancellationToken);
        }

        var phoneNumberResult = PhoneNumber.Create(identifier);
        if (!phoneNumberResult.IsError)
        {
            return await userRepository.GetByPhoneNumberAsync(phoneNumberResult.Value, cancellationToken);
        }

        return null;
    }

    private static VerifyOtpResponse CreateSuccessResponse()
        => VerifyOtpMapping.ToVerifyOtpResponse(SuccessMessage);
}
