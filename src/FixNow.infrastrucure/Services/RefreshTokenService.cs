using System.Security.Cryptography;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace FixNow.Infrastructure.Authentication;

public sealed class RefreshTokenService(
    IRefreshTokenRepository refreshTokenRepository,
    IRefreshTokenHasher refreshTokenHasher,
    IConfiguration configuration)
    : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IRefreshTokenHasher _refreshTokenHasher = refreshTokenHasher;

    public Result<RefreshTokenResult> Generate()
    {
        var expirationDays = configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays");

        if (expirationDays <= 0)
        {
            return Error.Unexpected(
                "Identity.RefreshTokenConfigurationInvalid",
                "Refresh token expiration must be greater than zero.");
        }

        Span<byte> bytes = stackalloc byte[64];

        RandomNumberGenerator.Fill(bytes);

        var token = Convert.ToBase64String(bytes);

        return new RefreshTokenResult(
            Token: token,
            ExpiresAt: DateTimeOffset.UtcNow.AddDays(expirationDays));
    }

    public async Task<Result<Success>> StoreAsync(
        Guid userId,
        RefreshTokenResult refreshToken,
        CancellationToken cancellationToken)
    {
        var refreshTokenResult = RefreshToken.Create(
            id: Guid.NewGuid(),
            userId: userId,
            token: _refreshTokenHasher.Hash(refreshToken.Token),
            expiresAt: refreshToken.ExpiresAt);

        if (refreshTokenResult.IsError)
            return refreshTokenResult.Errors;

        await _refreshTokenRepository.AddAsync(refreshTokenResult.Value, cancellationToken);

        return Result.Success;
    }
}
