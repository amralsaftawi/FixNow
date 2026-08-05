public sealed record AccessTokenResult(
    string Token,
    DateTimeOffset ExpiresAt);