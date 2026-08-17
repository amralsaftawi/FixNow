
public static class JobTimelineErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "JobTimeline.IdRequired",
            "Job timeline id is required.");

    public static readonly Error JobIdRequired =
        Error.Validation(
            "JobTimeline.JobIdRequired",
            "Job id is required.");

    public static readonly Error DescriptionRequired =
        Error.Validation(
            "JobTimeline.DescriptionRequired",
            "Description is required.");

    public static readonly Error DescriptionTooLong =
        Error.Validation(
            "JobTimeline.DescriptionTooLong",
            "Description cannot exceed 500 characters.");
}
