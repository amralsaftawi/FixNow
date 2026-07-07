
public static class CountryCodeErrors
{
    public static readonly Error Required =
        Error.Validation(
            "CountryCode.Required",
            "Country code is required.");

    public static readonly Error InvalidLength =
        Error.Validation(
            "CountryCode.InvalidLength",
            "Country code must consist of exactly 2 letters.");

    public static readonly Error InvalidFormat =
        Error.Validation(
            "CountryCode.InvalidFormat",
            "Country code must follow ISO 3166-1 Alpha-2 format.");
}