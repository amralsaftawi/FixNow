
namespace FixNow.Application.Features.Identity.Commands.Login;

public static class LoginMapping
{
    public static LoginResponse ToLoginResponse(
        this User entity,
        AccessTokenResult token)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(token);

        return new LoginResponse(
            UserId: entity.Id,
            AccessToken: token.AccessToken,
            RefreshToken: token.RefreshToken,
            ExpiresAt: token.ExpiresAt);
    }
}