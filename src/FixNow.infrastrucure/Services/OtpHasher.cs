using Microsoft.AspNetCore.Identity;

namespace FixNow.Infrastructure.Services;

public sealed class OtpHasher : IOtpHasher
{
    private static readonly OtpHasherUser HasherUser = new();
    private readonly PasswordHasher<OtpHasherUser> _passwordHasher = new();

    public string Hash(string otp)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(otp);

        return _passwordHasher.HashPassword(HasherUser, otp);
    }

    private sealed class OtpHasherUser;
}
