
public static class JobAdditionalChargeErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "JobAdditionalCharge.IdRequired",
            "Job additional charge id is required.");

    public static readonly Error JobIdRequired =
        Error.Validation(
            "JobAdditionalCharge.JobIdRequired",
            "Job id is required.");

    public static readonly Error DescriptionRequired =
        Error.Validation(
            "JobAdditionalCharge.DescriptionRequired",
            "Description is required.");

    public static readonly Error DescriptionTooLong =
        Error.Validation(
            "JobAdditionalCharge.DescriptionTooLong",
            "Description cannot exceed 500 characters.");

    public static readonly Error AmountRequired =
        Error.Validation(
            "JobAdditionalCharge.AmountRequired",
            "Amount is required.");
}
