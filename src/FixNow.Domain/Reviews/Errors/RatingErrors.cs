public static class RatingErrors
{
    public static readonly Error InvalidValue =
        Error.Validation(
            "Rating.InvalidValue",
            "Rating must be between 1 and 5.");
}