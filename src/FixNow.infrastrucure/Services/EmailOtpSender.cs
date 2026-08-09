using Microsoft.Extensions.Logging;

namespace FixNow.Infrastructure.Services;

public sealed class EmailOtpSender(ILogger<EmailOtpSender> logger) : IEmailOtpSender
{
    public Task<Result<Success>> SendAsync(
        string email,
        string otp,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("OTP requested for email {Email}.", email);

        return Task.FromResult<Result<Success>>(Result.Success);
    }
}
