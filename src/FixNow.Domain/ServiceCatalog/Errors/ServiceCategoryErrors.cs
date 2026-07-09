
public static class ServiceCategoryErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "ServiceCategory.IdRequired",
            "Service category id is required.");

    public static readonly Error NameRequired =
        Error.Validation(
            "ServiceCategory.NameRequired",
            "Service category name is required.");

    public static readonly Error NameTooLong =
        Error.Validation(
            "ServiceCategory.NameTooLong",
            "Service category name cannot exceed 100 characters.");

    public static readonly Error DescriptionTooLong =
        Error.Validation(
            "ServiceCategory.DescriptionTooLong",
            "Service category description cannot exceed 500 characters.");

    public static readonly Error IconKeyRequired =
        Error.Validation(
            "ServiceCategory.IconKeyRequired",
            "Service category icon is required.");

    public static readonly Error InvalidDisplayOrder =
        Error.Validation(
            "ServiceCategory.InvalidDisplayOrder",
            "Display order must be greater than or equal to zero.");

    public static readonly Error SameName =
        Error.Conflict(
            "ServiceCategory.SameName",
            "The new name is the same as the current name.");

    public static readonly Error SameDescription =
        Error.Conflict(
            "ServiceCategory.SameDescription",
            "The new description is the same as the current description.");

    public static readonly Error SameIcon =
        Error.Conflict(
            "ServiceCategory.SameIcon",
            "The new icon is the same as the current icon.");

    public static readonly Error SameDisplayOrder =
        Error.Conflict(
            "ServiceCategory.SameDisplayOrder",
            "The display order is already the same.");

    public static readonly Error AlreadyActive =
        Error.Conflict(
            "ServiceCategory.AlreadyActive",
            "Service category is already active.");

    public static readonly Error AlreadyInactive =
        Error.Conflict(
            "ServiceCategory.AlreadyInactive",
            "Service category is already inactive.");
}