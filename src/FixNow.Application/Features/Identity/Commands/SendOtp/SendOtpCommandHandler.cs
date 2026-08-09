using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Identity.Commands.SendOtp;

public sealed class SendOtpCommandHandler(
    IUserRepository userRepository,
    IOtpRepository otpRepository,
    IOtpGenerator otpGenerator,
    IOtpHasher otpHasher,
    IEmailOtpSender emailOtpSender,
    ISmsOtpSender smsOtpSender)
    : ICommandHandler<SendOtpCommand, Result<SendOtpResponse>>
{
    private const int MaxAttempts = 5;
    private const string SuccessMessage =
        "If the account exists, an OTP has been sent.";

    public async Task<Result<SendOtpResponse>> Handle(SendOtpCommand command,CancellationToken cancellationToken)
    {
        var recipient = await FindRecipientAsync( command.Identifier, cancellationToken);

        if (recipient is null)
            return CreateSuccessResponse();

        var existingOtpRecords = await otpRepository.GetActiveByUserAndPurposeAsync(
            recipient.User.Id,
            recipient.Purpose,
            cancellationToken);

        foreach (var otpRecord in existingOtpRecords)
        {
            var invalidateResult = otpRecord.Invalidate();

            if (invalidateResult.IsError)
                return invalidateResult.Errors;
        }

        await otpRepository.UpdateRangeAsync(existingOtpRecords,cancellationToken);

        var otpResult = otpGenerator.Generate();

        if (otpResult.IsError)
            return otpResult.Errors;

        var otp = otpResult.Value;

        var createOtpResult = OTPRecord.Create(
            Guid.NewGuid(),
            recipient.User.Id,
            otpHasher.Hash(otp.Code),
            recipient.Purpose,
            otp.ExpiresAt,
            MaxAttempts);

        if (createOtpResult.IsError)
            return createOtpResult.Errors;

        await otpRepository.AddAsync(
            createOtpResult.Value,
            cancellationToken);

        var sendResult = recipient.Purpose == OtpPurpose.EmailVerification
            ? await emailOtpSender.SendAsync(recipient.Destination, otp.Code, cancellationToken)
            : await smsOtpSender.SendAsync(recipient.Destination, otp.Code, cancellationToken);

        if (sendResult.IsError)
            return sendResult.Errors;

        return CreateSuccessResponse();
    }

    private async Task<OtpRecipient?> FindRecipientAsync(
        string identifier,
        CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(identifier);

        if (!emailResult.IsError)
        {
            var user = await userRepository.GetByEmailAsync(
                emailResult.Value,
                cancellationToken);

            return user is null
                ? null
                : new OtpRecipient(user, emailResult.Value, OtpPurpose.EmailVerification);
        }

        var phoneNumberResult = PhoneNumber.Create(identifier);

        if (!phoneNumberResult.IsError)
        {
            var userByPhone = await userRepository.GetByPhoneNumberAsync(
                phoneNumberResult.Value,
                cancellationToken);

            return userByPhone is null
                ? null
                : new OtpRecipient(userByPhone, phoneNumberResult.Value, OtpPurpose.PhoneVerification);
        }

        return null;
    }

    private static SendOtpResponse CreateSuccessResponse()
        => new(SuccessMessage);

    private sealed record OtpRecipient(
        User User,
        string Destination,
        OtpPurpose Purpose);
}
