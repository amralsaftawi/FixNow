using System.Security.Cryptography;

namespace FixNow.Infrastructure.Services;

public sealed class OtpGenerator : IOtpGenerator
{
    public string Generate()
        => RandomNumberGenerator.GetInt32(1_000_000).ToString("D6");
}
