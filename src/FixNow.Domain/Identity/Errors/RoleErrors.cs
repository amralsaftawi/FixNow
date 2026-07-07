public static class RoleErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "Role.IdRequired",
            "Role id is required.");

    public static readonly Error NameRequired =
        Error.Validation(
            "Role.NameRequired",
            "Role name is required.");

    public static readonly Error NameTooLong =
        Error.Validation(
            "Role.NameTooLong",
            "Role name cannot exceed 50 characters.");
}