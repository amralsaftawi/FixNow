public sealed record AccessTokenResult(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAt);