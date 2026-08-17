

public static class CustomerProfileErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "CustomerProfile.IdRequired",
            "Customer profile id is required.");

    public static readonly Error UserIdRequired =
        Error.Validation(
            "CustomerProfile.UserIdRequired",
            "User id is required.");

    public static readonly Error AddressRequired =
        Error.Validation(
            "CustomerProfile.AddressRequired",
            "Address is required.");

    public static readonly Error AddressAlreadyExists =
        Error.Conflict(
            "CustomerProfile.AddressAlreadyExists",
            "Address already exists.");

    public static readonly Error AddressNotFound =
        Error.NotFound(
            "CustomerProfile.AddressNotFound",
            "Address was not found.");

    public static readonly Error PaymentMethodRequired =
        Error.Validation(
            "CustomerProfile.PaymentMethodRequired",
            "Payment method is required.");

    public static readonly Error PaymentMethodAlreadyExists =
        Error.Conflict(
            "CustomerProfile.PaymentMethodAlreadyExists",
            "Payment method already exists.");

    public static readonly Error PaymentMethodNotFound =
        Error.NotFound(
            "CustomerProfile.PaymentMethodNotFound",
            "Payment method was not found.");

    public static readonly Error LatitudeInvalid =
        Error.Validation(
            "CustomerProfile.LatitudeInvalid",
            "Latitude must be between -90 and 90.");

    public static readonly Error LongitudeInvalid =
        Error.Validation(
            "CustomerProfile.LongitudeInvalid",
            "Longitude must be between -180 and 180.");

    public static readonly Error NotFound =
        Error.NotFound(
            "CustomerProfile.NotFound",
            "Customer profile was not found.");
}