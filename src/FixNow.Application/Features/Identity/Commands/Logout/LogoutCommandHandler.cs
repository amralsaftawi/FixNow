using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.Identity.Commands.Logout;

public sealed class LogoutCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IRefreshTokenHasher refreshTokenHasher)
    : ICommandHandler<LogoutCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle( LogoutCommand command, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepository.GetByTokenHashAsync(refreshTokenHasher.Hash(command.RefreshToken),cancellationToken);

        if (refreshToken is null)
            return IdentityErrors.InvalidRefreshToken;

        if (refreshToken.IsRevoked)
            return IdentityErrors.RefreshTokenRevoked;

        if (refreshToken.IsExpired)
            return IdentityErrors.RefreshTokenExpired;

        var revokeResult = refreshToken.Revoke();

        if (revokeResult.IsError)
            return revokeResult.Errors;

        var wasRevoked = await refreshTokenRepository.TryRevokeAsync(refreshToken,cancellationToken);

        if (!wasRevoked)
            return IdentityErrors.RefreshTokenRevoked;

        return Result.Success;
    }
}
