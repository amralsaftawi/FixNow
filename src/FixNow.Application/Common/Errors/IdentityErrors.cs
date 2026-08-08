namespace FixNow.Domain.Common.Errors;

public static class IdentityErrors
{
    public static readonly Error EmailAlreadyExists =
        Error.Conflict(
            code: "Identity.EmailAlreadyExists",
            description: "A user with the same email already exists.");

    public static readonly Error PhoneNumberAlreadyExists =
        Error.Conflict(
            code: "Identity.PhoneNumberAlreadyExists",
            description: "A user with the same phone number already exists.");

    public static readonly Error InvalidCredentials =
        Error.Unauthorized(
            code: "Identity.InvalidCredentials",
            description: "Invalid email or password.");

    public static readonly Error IncorrectPassword =
        Error.Unauthorized(
            code: "Identity.IncorrectPassword",
            description: "The current password is incorrect.");

    public static readonly Error InvalidToken =
        Error.Unauthorized(
            code: "Identity.InvalidToken",
            description: "The provided token is invalid.");

    public static readonly Error EmailNotVerified =
        Error.Forbidden(
            code: "Identity.EmailNotVerified",
            description: "Email address has not been verified.");

public static readonly Error AccountNotActive =
    Error.Forbidden(
        "Identity.AccountNotActive",
        "Your account is not active.");

        public static readonly Error InvalidRefreshToken =
    Error.Unauthorized(
        "Identity.InvalidRefreshToken",
        "The refresh token is invalid.");

public static readonly Error RefreshTokenRevoked =
    Error.Unauthorized(
        "Identity.RefreshTokenRevoked",
        "The refresh token has already been revoked.");

public static readonly Error RefreshTokenExpired =
    Error.Unauthorized(
        "Identity.RefreshTokenExpired",
        "The refresh token has expired.");

        public static readonly Error Unauthorized =
    Error.Unauthorized(
        "Identity.Unauthorized",
        "You must be authenticated to access the current user.");

        public static readonly Error InvalidResetPasswordRequest =
    Error.Unauthorized(
        "Identity.InvalidResetPasswordRequest",
        "The password reset request is invalid.");
}