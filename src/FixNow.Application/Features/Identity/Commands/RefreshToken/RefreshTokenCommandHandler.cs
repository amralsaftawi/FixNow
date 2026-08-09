using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.Identity.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IRefreshTokenService refreshTokenService,
    IRefreshTokenHasher refreshTokenHasher,
    ITokenService tokenService,
    IUserRepository userRepository)
    : ICommandHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
    private readonly IRefreshTokenHasher _refreshTokenHasher = refreshTokenHasher;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand command,CancellationToken cancellationToken)
    {
        // 1. Find Refresh Token
        var refreshToken = await _refreshTokenRepository.GetWithUserByTokenHashAsync(_refreshTokenHasher.Hash(command.RefreshToken),cancellationToken);

        if (refreshToken is null)
            return IdentityErrors.InvalidRefreshToken;

        // 2. Check revoked
        if (refreshToken.IsRevoked)
            return IdentityErrors.RefreshTokenRevoked;

        // 3. Check expired
        if (refreshToken.IsExpired)
            return IdentityErrors.RefreshTokenExpired;

        var user = refreshToken.User;

        // 4. Check account status
        if (user.AccountStatus != AccountStatus.Active)
            return IdentityErrors.AccountNotVerified;

        // 5. Revoke current refresh token
        var revokeResult = refreshToken.Revoke();

        if (revokeResult.IsError)
            return revokeResult.Errors;

        var wasRevoked = await _refreshTokenRepository.TryRevokeAsync(refreshToken,cancellationToken);

        if (!wasRevoked)
            return IdentityErrors.RefreshTokenRevoked;

        // 6. Generate Access Token
        var roles = await _userRepository.GetRolesByUserIdAsync(user.Id, cancellationToken);

        var accessTokenResult = _tokenService.GenerateAccessToken(user, roles);

        if (accessTokenResult.IsError)
            return accessTokenResult.Errors;

        // 7. Generate Refresh Token
        var newRefreshTokenResult = _refreshTokenService.Generate();

        if (newRefreshTokenResult.IsError)
            return newRefreshTokenResult.Errors;

        // 8. Store New Refresh Token
        var storeRefreshTokenResult = await _refreshTokenService.StoreAsync( user.Id,newRefreshTokenResult.Value,cancellationToken);

        if (storeRefreshTokenResult.IsError)
            return storeRefreshTokenResult.Errors;

        // 9. Return Response
        return (
            accessTokenResult.Value,
            newRefreshTokenResult.Value
        ).ToRefreshTokenResponse();
    }
}
