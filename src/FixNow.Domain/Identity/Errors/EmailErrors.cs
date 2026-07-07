
public static class EmailErrors
{
    public static readonly Error Required =
        Error.Validation(
            "Email.Required",
            "Email is required.");

    public static readonly Error InvalidFormat =
        Error.Validation(
            "Email.InvalidFormat",
            "Email format is invalid.");

    public static readonly Error TooLong =
        Error.Validation(
            "Email.TooLong",
            "Email cannot exceed 320 characters.");
}