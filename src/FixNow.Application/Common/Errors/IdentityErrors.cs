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

    public static readonly Error InvalidToken =
        Error.Unauthorized(
            code: "Identity.InvalidToken",
            description: "The provided token is invalid.");

    public static readonly Error EmailNotVerified =
        Error.Forbidden(
            code: "Identity.EmailNotVerified",
            description: "Email address has not been verified.");
}