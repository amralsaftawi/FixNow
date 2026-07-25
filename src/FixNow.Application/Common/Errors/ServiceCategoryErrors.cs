using FixNow.Domain.Common.Errors;

public static class ServiceCategoryErrors
{
    public static readonly Error NameAlreadyExists =
        Error.Conflict(
            code: "ServiceCategory.Name.AlreadyExists",
            description: "A service category with the same name already exists.");

    public static readonly Error SameName =
        Error.Validation(
            code: "ServiceCategory.Name.Unchanged",
            description: "The new service category name is the same as the current name.");

    public static readonly Error SameDescription =
        Error.Validation(
            code: "ServiceCategory.Description.Unchanged",
            description: "The new service category description is the same as the current description.");

  
    public static readonly Error SameIcon =
        Error.Validation(
            code: "ServiceCategory.IconKey.Unchanged",
            description: "The new service category icon is the same as the current icon.");

    public static readonly Error InvalidDisplayOrder =
        Error.Validation(
            code: "ServiceCategory.DisplayOrder.Invalid",
            description: "Display order cannot be negative.");

    public static readonly Error SameDisplayOrder =
        Error.Validation(
            code: "ServiceCategory.DisplayOrder.Unchanged",
            description: "The new display order is the same as the current display order.");

    public static readonly Error AlreadyActive =
        Error.Conflict(
            code: "ServiceCategory.AlreadyActive",
            description: "The service category is already active.");

    public static readonly Error AlreadyInactive =
        Error.Conflict(
            code: "ServiceCategory.AlreadyInactive",
            description: "The service category is already inactive.");

    public static readonly Error NotFound =
        Error.NotFound(
            code: "ServiceCategory.NotFound",
            description: "The requested service category was not found.");
}