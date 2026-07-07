
public static class UserRoleErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "UserRole.IdRequired",
            "User role id is required.");

    public static readonly Error UserIdRequired =
        Error.Validation(
            "UserRole.UserIdRequired",
            "User id is required.");

    public static readonly Error RoleIdRequired =
        Error.Validation(
            "UserRole.RoleIdRequired",
            "Role id is required.");

   public static readonly Error RevokedByRequired =
      Error.Validation(
        "UserRole.RevokedByRequired",
        "The user who revoked this role is required.");
    public static readonly Error AlreadyRevoked =
        Error.Conflict(
            "UserRole.AlreadyRevoked",
            "The role assignment has already been revoked.");

    public static readonly Error AlreadyActive =
        Error.Conflict(
            "UserRole.AlreadyActive",
            "The role assignment is already active.");
}