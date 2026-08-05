using Microsoft.AspNetCore.Identity;

namespace FixNow.Infrastructure.Services;

public sealed class PasswordHasherService : IPasswordHasher
{
    private static readonly PasswordHasherUser HasherUser = new();

    private readonly PasswordHasher<PasswordHasherUser> _passwordHasher = new();

    public string Hash(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        return _passwordHasher.HashPassword(HasherUser, password);
    }

    public bool Verify(string password, string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        return _passwordHasher.VerifyHashedPassword(
                HasherUser,
                passwordHash,
                password)
            != PasswordVerificationResult.Failed;
    }

    private sealed class PasswordHasherUser;
}
