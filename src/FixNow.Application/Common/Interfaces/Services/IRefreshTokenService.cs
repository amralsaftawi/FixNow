public interface IRefreshTokenService
{
    Result<RefreshTokenResult> Generate();

    Task<Result<Success>> StoreAsync(
        Guid userId,
        RefreshTokenResult refreshToken,
        CancellationToken cancellationToken);
}
