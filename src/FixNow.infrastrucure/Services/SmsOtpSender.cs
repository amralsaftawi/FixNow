using Microsoft.Extensions.Logging;

namespace FixNow.Infrastructure.Services;

public sealed class SmsOtpSender(ILogger<SmsOtpSender> logger) : ISmsOtpSender
{
    public Task<Result<Success>> SendAsync(
        string phoneNumber,
        string otp,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("OTP {Otp} sent to phone number {PhoneNumber}.", otp, phoneNumber);

        return Task.FromResult<Result<Success>>(Result.Success);
    }
}
