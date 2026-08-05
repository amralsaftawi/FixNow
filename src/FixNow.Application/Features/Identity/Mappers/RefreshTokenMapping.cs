namespace FixNow.Application.Features.Identity.Commands.RefreshToken;

public static class RefreshTokenMapping
{
    public static RefreshTokenResponse ToRefreshTokenResponse(
        this (
            AccessTokenResult AccessToken,
            RefreshTokenResult RefreshToken
        ) result)
    {
        return new RefreshTokenResponse(
            AccessToken: result.AccessToken.Token,
            AccessTokenExpiresAt: result.AccessToken.ExpiresAt,
            RefreshToken: result.RefreshToken.Token,
            RefreshTokenExpiresAt: result.RefreshToken.ExpiresAt);
    }
}