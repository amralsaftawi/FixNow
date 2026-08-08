
public static class CityErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "City.IdRequired",
            "City id is required.");

    public static readonly Error CountryRequired =
        Error.Validation(
            "City.CountryRequired",
            "Country is required.");

    public static readonly Error NameRequired =
        Error.Validation(
            "City.NameRequired",
            "City name is required.");

    public static readonly Error NameTooLong =
        Error.Validation(
            "City.NameTooLong",
            "City name cannot exceed 100 characters.");

    public static readonly Error NotFound =
        Error.NotFound(
            "City.NotFound",
            "City was not found.");
}
