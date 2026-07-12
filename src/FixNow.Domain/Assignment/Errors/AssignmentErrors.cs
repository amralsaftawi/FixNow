
public static class AssignmentErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "Assignment.IdRequired",
            "Assignment id is required.");

    public static readonly Error ServiceRequestIdRequired =
        Error.Validation(
            "Assignment.ServiceRequestIdRequired",
            "Service request id is required.");

    public static readonly Error TechnicianProfileIdRequired =
        Error.Validation(
            "Assignment.TechnicianProfileIdRequired",
            "Technician profile id is required.");

    public static readonly Error AlreadyAccepted =
        Error.Conflict(
            "Assignment.AlreadyAccepted",
            "Assignment has already been accepted.");

    public static readonly Error AlreadyRejected =
        Error.Conflict(
            "Assignment.AlreadyRejected",
            "Assignment has already been rejected.");

    public static readonly Error AlreadyCompleted =
        Error.Conflict(
            "Assignment.AlreadyCompleted",
            "Assignment has already been completed.");

    public static readonly Error AlreadyCancelled =
        Error.Conflict(
            "Assignment.AlreadyCancelled",
            "Assignment has already been cancelled.");

    public static readonly Error InvalidStatusTransition =
        Error.Conflict(
            "Assignment.InvalidStatusTransition",
            "The requested status transition is not allowed.");

    public static readonly Error RejectReasonRequired =
        Error.Validation(
            "Assignment.RejectReasonRequired",
            "Reject reason is required.");
}