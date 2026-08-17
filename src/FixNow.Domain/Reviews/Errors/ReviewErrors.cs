
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

    public static readonly Error AlreadyRated =
        Error.Conflict(
            "Review.AlreadyRated",
            "A review for this job already exists.");

    public static readonly Error JobNotCompleted =
        Error.Conflict(
            "Review.JobNotCompleted",
            "Only completed jobs can be rated.");

    public static readonly Error CommentRequired =
        Error.Validation(
            "Review.CommentRequired",
            "Comment is required.");

    public static readonly Error CommentEmpty =
        Error.Validation(
            "Review.CommentEmpty",
            "Comment cannot be empty.");

    public static readonly Error AlreadyHidden =
        Error.Conflict(
            "Review.AlreadyHidden",
            "Review is already hidden.");

    public static readonly Error AlreadyVisible =
        Error.Conflict(
            "Review.AlreadyVisible",
            "Review is already visible.");

    public static readonly Error NotFound =
        Error.NotFound(
            "Review.NotFound",
            "Review was not found.");
}