public static class CustomerRatingErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "CustomerRating.IdRequired",
            "Customer rating id is required.");

    public static readonly Error JobIdRequired =
        Error.Validation(
            "CustomerRating.JobIdRequired",
            "Job id is required.");

    public static readonly Error TechnicianProfileIdRequired =
        Error.Validation(
            "CustomerRating.TechnicianProfileIdRequired",
            "Technician profile id is required.");

    public static readonly Error CustomerProfileIdRequired =
        Error.Validation(
            "CustomerRating.CustomerProfileIdRequired",
            "Customer profile id is required.");

    public static readonly Error InvalidRating =
        Error.Validation(
            "CustomerRating.InvalidRating",
            "Rating must be between 1 and 5.");

    public static readonly Error AlreadyRated =
        Error.Conflict(
            "CustomerRating.AlreadyRated",
            "A customer rating for this job already exists.");

    public static readonly Error JobNotCompleted =
        Error.Conflict(
            "CustomerRating.JobNotCompleted",
            "Only completed jobs can have customer ratings.");

    public static readonly Error CannotRateOwnCustomer =
        Error.Conflict(
            "CustomerRating.CannotRateOwnCustomer",
            "A technician cannot rate a customer they have not worked for.");
}
