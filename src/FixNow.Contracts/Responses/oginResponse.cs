public sealed record LoginResponse
(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,

    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt,

    string TokenType
);