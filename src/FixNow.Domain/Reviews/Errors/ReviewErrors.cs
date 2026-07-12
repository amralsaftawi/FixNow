
public static class ReviewErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "Review.IdRequired",
            "Review id is required.");

    public static readonly Error AssignmentIdRequired =
        Error.Validation(
            "Review.AssignmentIdRequired",
            "Assignment id is required.");

    public static readonly Error ServiceRequestIdRequired =
        Error.Validation(
            "Review.ServiceRequestIdRequired",
            "Service request id is required.");

    public static readonly Error CustomerProfileIdRequired =
        Error.Validation(
            "Review.CustomerProfileIdRequired",
            "Customer profile id is required.");

    public static readonly Error TechnicianProfileIdRequired =
        Error.Validation(
            "Review.TechnicianProfileIdRequired",
            "Technician profile id is required.");

    public static readonly Error CommentTooLong =
        Error.Validation(
            "Review.CommentTooLong",
            "Comment cannot exceed 1000 characters.");

    public static readonly Error NothingChanged =
        Error.Conflict(
            "Review.NothingChanged",
            "No changes were made.");
}