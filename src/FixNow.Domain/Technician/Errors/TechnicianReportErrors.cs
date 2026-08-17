public static class TechnicianReportErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "TechnicianReport.IdRequired",
            "Technician report id is required.");

    public static readonly Error TechnicianProfileIdRequired =
        Error.Validation(
            "TechnicianReport.TechnicianProfileIdRequired",
            "Technician profile id is required.");

    public static readonly Error ReporterUserIdRequired =
        Error.Validation(
            "TechnicianReport.ReporterUserIdRequired",
            "Reporter user id is required.");

    public static readonly Error ReasonRequired =
        Error.Validation(
            "TechnicianReport.ReasonRequired",
            "Report reason is required.");

    public static readonly Error TechnicianNotFound =
        Error.NotFound(
            "TechnicianReport.TechnicianNotFound",
            "The reported technician was not found.");

    public static readonly Error AlreadyReported =
        Error.Conflict(
            "TechnicianReport.AlreadyReported",
            "You have already reported this technician.");

    public static readonly Error CannotReportSelf =
        Error.Conflict(
            "TechnicianReport.CannotReportSelf",
            "You cannot report yourself.");

    public static readonly Error DescriptionTooLong =
        Error.Validation(
            "TechnicianReport.DescriptionTooLong",
            "Description cannot exceed 1000 characters.");
}
