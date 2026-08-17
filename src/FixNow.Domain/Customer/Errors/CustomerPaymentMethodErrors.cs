
public static class CustomerPaymentMethodErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "CustomerPaymentMethod.IdRequired",
            "Payment method id is required.");

    public static readonly Error CustomerProfileIdRequired =
        Error.Validation(
            "CustomerPaymentMethod.CustomerProfileIdRequired",
            "Customer profile id is required.");

    public static readonly Error TypeRequired =
        Error.Validation(
            "CustomerPaymentMethod.TypeRequired",
            "Payment method type is required.");

    public static readonly Error AlreadyDefault =
        Error.Conflict(
            "CustomerPaymentMethod.AlreadyDefault",
            "This payment method is already the default payment method.");
}
