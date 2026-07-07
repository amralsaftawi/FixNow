public static class RefreshTokenErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "RefreshToken.IdRequired",
            "Refresh token id is required.");

    public static readonly Error UserIdRequired =
        Error.Validation(
            "RefreshToken.UserIdRequired",
            "User id is required.");

    public static readonly Error TokenRequired =
        Error.Validation(
            "RefreshToken.TokenRequired",
            "Refresh token is required.");

    public static readonly Error TokenTooLong =
        Error.Validation(
            "RefreshToken.TokenTooLong",
            "Refresh token exceeds the maximum allowed length.");

    public static readonly Error InvalidExpirationDate =
        Error.Validation(
            "RefreshToken.InvalidExpirationDate",
            "Expiration date must be in the future.");

    public static readonly Error AlreadyRevoked =
        Error.Conflict(
            "RefreshToken.AlreadyRevoked",
            "The refresh token has already been revoked.");

    public static readonly Error AlreadyExpired =
        Error.Conflict(
            "RefreshToken.AlreadyExpired",
            "The refresh token has already expired.");

    public static readonly Error AlreadyActive =
        Error.Conflict(
            "RefreshToken.AlreadyActive",
            "The refresh token is already active.");

    public static readonly Error CannotRevokeExpiredToken =
        Error.Conflict(
            "RefreshToken.CannotRevokeExpiredToken",
            "Expired refresh tokens cannot be revoked.");
}