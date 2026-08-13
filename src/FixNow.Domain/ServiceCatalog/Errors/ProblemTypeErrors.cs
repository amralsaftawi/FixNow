
public static class ProblemTypeErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "ProblemType.IdRequired",
            "Problem type id is required.");

    public static readonly Error NameRequired =
        Error.Validation(
            "ProblemType.NameRequired",
            "Problem type name is required.");

    public static readonly Error NameTooLong =
        Error.Validation(
            "ProblemType.NameTooLong",
            "Problem type name cannot exceed 100 characters.");

    public static readonly Error ServiceCategoryIdRequired =
        Error.Validation(
            "ProblemType.ServiceCategoryIdRequired",
            "Service category id is required.");

    public static readonly Error NotFound =
        Error.NotFound(
            "ProblemType.NotFound",
            "The problem type was not found.");
}
