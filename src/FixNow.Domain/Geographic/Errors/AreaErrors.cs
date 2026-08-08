
public static class AreaErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "Area.IdRequired",
            "Area id is required.");

    public static readonly Error CityRequired =
        Error.Validation(
            "Area.CityRequired",
            "City is required.");

    public static readonly Error NameRequired =
        Error.Validation(
            "Area.NameRequired",
            "Area name is required.");

    public static readonly Error NameTooLong =
        Error.Validation(
            "Area.NameTooLong",
            "Area name cannot exceed 100 characters.");

    public static readonly Error NotFound =
        Error.NotFound(
            "Area.NotFound",
            "Area was not found.");
}
