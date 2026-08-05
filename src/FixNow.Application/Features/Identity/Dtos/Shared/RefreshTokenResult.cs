public sealed record RefreshTokenResult(
    string Token,
    DateTimeOffset ExpiresAt);