
public static class JobErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "Job.IdRequired",
            "Job id is required.");

    public static readonly Error ServiceRequestIdRequired =
        Error.Validation(
            "Job.ServiceRequestIdRequired",
            "Service request id is required.");

    public static readonly Error TechnicianProfileIdRequired =
        Error.Validation(
            "Job.TechnicianProfileIdRequired",
            "Technician profile id is required.");

    public static readonly Error AlreadyConverted =
        Error.Conflict(
            "Job.AlreadyConverted",
            "The service request has already been converted to a job.");

    public static readonly Error RequestCancelled =
        Error.Conflict(
            "Job.RequestCancelled",
            "A cancelled service request cannot be converted to a job.");

    public static readonly Error RequestCompleted =
        Error.Conflict(
            "Job.RequestCompleted",
            "A completed service request cannot be converted to a job.");

    public static readonly Error NotFound =
        Error.NotFound(
            "Job.NotFound",
            "The job was not found.");

    public static readonly Error InvalidStatusTransition =
        Error.Conflict(
            "Job.InvalidStatusTransition",
            "The requested status transition is not allowed.");

    public static readonly Error SameStatus =
        Error.Conflict(
            "Job.SameStatus",
            "The job is already in the requested status.");

    public static readonly Error JobNotCompleted =
        Error.Conflict(
            "Job.NotCompleted",
            "Only a completed job can be confirmed.");

    public static readonly Error CompletionAlreadyConfirmed =
        Error.Conflict(
            "Job.CompletionAlreadyConfirmed",
            "The completion has already been confirmed.");

    public static readonly Error AdditionalChargeRequired =
        Error.Validation(
            "Job.AdditionalChargeRequired",
            "Additional charge is required.");

    public static readonly Error AdditionalChargeNotAllowed =
        Error.Conflict(
            "Job.AdditionalChargeNotAllowed",
            "Additional charges cannot be added to a completed or cancelled job.");

    public static readonly Error FinalPriceNotAllowed =
        Error.Conflict(
            "Job.FinalPriceNotAllowed",
            "The final price can only be finalized when the job is completed.");
}
