public interface ITokenService
{
    Result<AccessTokenResult> GenerateAccessToken(User user, List<Role> roles);
}