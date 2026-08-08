public static class CustomerProfileErrors
{
    public static readonly Error AlreadyExists =
        Error.Conflict(
            code: "CustomerProfile.AlreadyExists",
            description: "The customer profile already exists.");

    public static readonly Error NotFound =
        Error.NotFound(
            code: "CustomerProfile.NotFound",
            description: "The customer profile was not found.");

    public static readonly Error AddressNotFound =
        Error.NotFound(
            code: "CustomerProfile.AddressNotFound",
            description: "The address was not found.");

    public static readonly Error CurrentLocationNotFound =
        Error.NotFound(
            code: "CustomerProfile.CurrentLocationNotFound",
            description: "The current location has not been set yet.");
}
