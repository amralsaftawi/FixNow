

public static class PasswordHashErrors
{
    public static readonly Error Required =
        Error.Validation(
            "PasswordHash.Required",
            "Password hash is required.");

    public static readonly Error InvalidHash =
        Error.Validation(
            "PasswordHash.Invalid",
            "Password hash is invalid.");
}