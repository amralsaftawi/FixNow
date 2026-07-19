public interface ITokenProvider
{
    Task<AccessTokenResult> GenerateAsync(
        TokenUserInfo user,
        CancellationToken cancellationToken = default);
}