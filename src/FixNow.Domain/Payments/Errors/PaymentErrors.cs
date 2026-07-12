
public static class PaymentErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "Payment.IdRequired",
            "Payment id is required.");

    public static readonly Error AssignmentIdRequired =
        Error.Validation(
            "Payment.AssignmentIdRequired",
            "Assignment id is required.");

    public static readonly Error CustomerProfileIdRequired =
        Error.Validation(
            "Payment.CustomerProfileIdRequired",
            "Customer profile id is required.");

    public static readonly Error AlreadyPaid =
        Error.Conflict(
            "Payment.AlreadyPaid",
            "Payment has already been completed.");

    public static readonly Error AlreadyFailed =
        Error.Conflict(
            "Payment.AlreadyFailed",
            "Payment has already failed.");

    public static readonly Error AlreadyRefunded =
        Error.Conflict(
            "Payment.AlreadyRefunded",
            "Payment has already been refunded.");

    public static readonly Error InvalidStatusTransition =
        Error.Conflict(
            "Payment.InvalidStatusTransition",
            "The requested payment status transition is not allowed.");
}