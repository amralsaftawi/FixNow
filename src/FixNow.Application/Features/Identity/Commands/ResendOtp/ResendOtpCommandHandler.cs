using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Authentication;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.Identity.Commands.ResendOtp;

public sealed class ResendOtpCommandHandler(
    IUserRepository userRepository,
    IOtpRepository otpRepository,
    IOtpHasher otpHasher,
    IOtpGenerator otpGenerator,
    IOtpSender otpSender,
    global::IUnitOfWork unitOfWork)
    : ICommandHandler<ResendOtpCommand, Result<ResendOtpResponse>>
{
     private const int MaxAttempts = 5;
    private const string SuccessMessage =
        "If the account exists, a new OTP has been sent.";

    private readonly IUserRepository _userRepository = userRepository;
    private readonly IOtpRepository _otpRepository = otpRepository;
    private readonly IOtpHasher _otpHasher = otpHasher;
    private readonly IOtpGenerator _otpGenerator = otpGenerator;
    private readonly IOtpSender _otpSender = otpSender;
    private readonly global::IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<ResendOtpResponse>> Handle(
        ResendOtpCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Parse OTP purpose.
        if (!Enum.TryParse<OtpPurpose>(
                command.Purpose,
                ignoreCase: true,
                out var purpose))
        {
            return OTPRecordErrors.InvalidPurpose;
        }

        // 2. Find user by email or phone.
        var user = await FindUserAsync(
            command.Identifier,
            cancellationToken);

        // 3. Do not reveal whether the account exists.
        if (user is null)
        {
            return CreateSuccessResponse();
        }

        // 4. Check whether the requested verification is still required.
        if (purpose == OtpPurpose.EmailVerification &&
            user.IsEmailVerified)
        {
            return CreateSuccessResponse();
        }

        if (purpose == OtpPurpose.PhoneVerification &&user.IsPhoneNumberVerified)
        {
            return CreateSuccessResponse();
        }

        // 5. Get the latest OTP for this user and purpose.
        var latestOtp = await _otpRepository.GetLatestAsync(
            user.Id,
            purpose,
            cancellationToken);

        // 6. Prevent unnecessary OTP regeneration while
        //    an existing OTP is still active.
        if (latestOtp is not null &&
            !latestOtp.IsExpired &&
            !latestOtp.IsVerified &&
            !latestOtp.IsInvalidated)
        {
            return CreateSuccessResponse();
        }

        // 7. Generate a new OTP.
        var otpResult = _otpGenerator.Generate();

        if (otpResult.IsError)
        {
            return otpResult.Errors;
        }

        var otp = otpResult.Value;

        // 8. Hash the OTP before persistence.
        var codeHash = _otpHasher.Hash(otp.Code);

        // 9. Create the OTP record.
        var otpRecordResult = OTPRecord.Create(
            id: Guid.NewGuid(),
            userId: user.Id,
            codeHash: codeHash,
            purpose: purpose,
            expiresAt: otp.ExpiresAt,
            MaxAttempts);

        if (otpRecordResult.IsError)
        {
            return otpRecordResult.Errors;
        }

        var otpRecord = otpRecordResult.Value;

        // 10. Persist the OTP record.
        await _otpRepository.AddAsync(
            otpRecord,
            cancellationToken);

        // 11. Send OTP through the appropriate channel.
        var sendResult = await _otpSender.SendAsync(
            user,
            otp.Code,
            purpose,
            cancellationToken);

        if (sendResult.IsError)
        {
            return sendResult.Errors;
        }

        // 12. Save all changes.
        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // 13. Return generic success response.
        return CreateSuccessResponse();
    }

    private async Task<User?> FindUserAsync(
        string identifier,
        CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(identifier);

        if (!emailResult.IsError)
        {
            return await _userRepository.GetByEmailAsync(
                emailResult.Value,
                cancellationToken);
        }

        var phoneNumberResult = PhoneNumber.Create(identifier);

        if (!phoneNumberResult.IsError)
        {
            return await _userRepository.GetByPhoneNumberAsync(
                phoneNumberResult.Value,
                cancellationToken);
        }

        return null;
    }

    private static ResendOtpResponse CreateSuccessResponse()
        => ResendOtpMapping.ToResendOtpResponse(
            SuccessMessage);
}