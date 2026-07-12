
public static class ServiceRequestTimelineErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "ServiceRequestTimeline.IdRequired",
            "Service request timeline id is required.");

    public static readonly Error ServiceRequestIdRequired =
        Error.Validation(
            "ServiceRequestTimeline.ServiceRequestIdRequired",
            "Service request id is required.");

    public static readonly Error DescriptionRequired =
        Error.Validation(
            "ServiceRequestTimeline.DescriptionRequired",
            "Description is required.");

    public static readonly Error DescriptionTooLong =
        Error.Validation(
            "ServiceRequestTimeline.DescriptionTooLong",
            "Description cannot exceed 500 characters.");
}