
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

    public static readonly Error JobNotCompleted =
        Error.Conflict(
            "Payment.JobNotCompleted",
            "A cash payment can only be recorded for a completed job.");

    public static readonly Error FinalPriceNotResolved =
        Error.Conflict(
            "Payment.FinalPriceNotResolved",
            "The final job price could not be resolved.");

    public static readonly Error AlreadyExists =
        Error.Conflict(
            "Payment.AlreadyExists",
            "A payment has already been recorded for this job.");

    public static readonly Error ActivePaymentAlreadyExists =
        Error.Conflict(
            "Payment.ActivePaymentAlreadyExists",
            "An active payment already exists for this job. Please complete or cancel it before starting a new one.");

    public static readonly Error ProviderNotConfigured =
        Error.Failure(
            "Payment.ProviderNotConfigured",
            "The online payment provider is not configured.");

    public static readonly Error NotFound =
        Error.NotFound(
            "Payment.NotFound",
            "The specified payment was not found.");

    public static readonly Error PaymentNotProcessable =
        Error.Conflict(
            "Payment.NotProcessable",
            "This payment cannot be processed. Only pending online payments are eligible for processing.");

    public static readonly Error AmountMismatch =
        Error.Conflict(
            "Payment.AmountMismatch",
            "The confirmed amount does not match the expected payment amount.");

    public static readonly Error ConfirmationNotAvailable =
        Error.Failure(
            "Payment.ConfirmationNotAvailable",
            "Payment confirmation is not available. The payment provider has not been configured.");

    public static readonly Error FailureNotAvailable =
        Error.Failure(
            "Payment.FailureNotAvailable",
            "Payment failure handling is not available. The payment provider has not been configured.");

    public static readonly Error RefundNotAvailable =
        Error.Failure(
            "Payment.RefundNotAvailable",
            "Payment refund is not available. The payment provider has not been configured.");
}