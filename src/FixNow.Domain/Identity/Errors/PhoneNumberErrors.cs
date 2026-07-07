
public static class PhoneNumberErrors
{
    public static readonly Error Required =
        Error.Validation(
            "PhoneNumber.Required",
            "Phone number is required.");

    public static readonly Error MustStartWithPlus =
        Error.Validation(
            "PhoneNumber.MustStartWithPlus",
            "Phone number must start with '+' according to E.164.");

    public static readonly Error InvalidFormat =
        Error.Validation(
            "PhoneNumber.InvalidFormat",
            "Phone number is not a valid E.164 phone number.");
}