using FixNow.Application.Common.Interfaces.Authentication;

namespace FixNow.Infrastructure.Services.Otp;

public sealed class OtpSender : IOtpSender
{
    public Task<Result<Success>> SendAsync(
        User user,
        string otp,
        OtpPurpose purpose,
        CancellationToken cancellationToken)
    {
        // TODO:
        // Route the OTP to the appropriate delivery provider
        // based on the OTP purpose / user's verified contact information.

        return Task.FromResult<Result<Success>>(
            Result.Success);
    }
}