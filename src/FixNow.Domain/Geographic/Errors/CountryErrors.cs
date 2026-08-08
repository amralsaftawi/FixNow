
public static class CountryErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "Country.IdRequired",
            "Country id is required.");

    public static readonly Error NameRequired =
        Error.Validation(
            "Country.NameRequired",
            "Country name is required.");

    public static readonly Error NameTooLong =
        Error.Validation(
            "Country.NameTooLong",
            "Country name cannot exceed 100 characters.");

    public static readonly Error NameAlreadyExists =
        Error.Conflict(
            "Country.NameAlreadyExists",
            "A country with this name already exists.");

    public static readonly Error NotFound =
        Error.NotFound(
            "Country.NotFound",
            "Country was not found.");
}
