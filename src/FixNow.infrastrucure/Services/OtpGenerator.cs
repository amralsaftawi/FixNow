using System.Security.Cryptography;

namespace FixNow.Infrastructure.Services;

public sealed class OtpGenerator : IOtpGenerator
{
    private static readonly TimeSpan Expiration = TimeSpan.FromMinutes(5);

    public Result<OtpResult> Generate()
    {
        var code = RandomNumberGenerator
            .GetInt32(1_000_000)
            .ToString("D6");

        return new OtpResult(
            code,
            DateTimeOffset.UtcNow.Add(Expiration));
    }
}
