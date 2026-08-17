public static class ReviewReportErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "ReviewReport.IdRequired",
            "Review report id is required.");

    public static readonly Error ReviewIdRequired =
        Error.Validation(
            "ReviewReport.ReviewIdRequired",
            "Review id is required.");

    public static readonly Error ReporterUserIdRequired =
        Error.Validation(
            "ReviewReport.ReporterUserIdRequired",
            "Reporter user id is required.");

    public static readonly Error ReasonRequired =
        Error.Validation(
            "ReviewReport.ReasonRequired",
            "Report reason is required.");

    public static readonly Error ReviewNotFound =
        Error.NotFound(
            "ReviewReport.ReviewNotFound",
            "The reported review was not found.");

    public static readonly Error AlreadyReported =
        Error.Conflict(
            "ReviewReport.AlreadyReported",
            "You have already reported this review.");

    public static readonly Error CannotReportOwnReview =
        Error.Conflict(
            "ReviewReport.CannotReportOwnReview",
            "You cannot report your own review.");

    public static readonly Error DescriptionTooLong =
        Error.Validation(
            "ReviewReport.DescriptionTooLong",
            "Description cannot exceed 1000 characters.");
}
