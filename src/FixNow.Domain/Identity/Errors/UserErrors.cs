
public static class UserErrors
{
    // =========================
    // Creation Errors
    // =========================

    public static readonly Error IdRequired =
        Error.Validation("User.IdRequired", "User id is required.");

    public static readonly Error FirstNameRequired =
        Error.Validation("User.FirstNameRequired", "First name is required.");

    public static readonly Error LastNameRequired =
        Error.Validation("User.LastNameRequired", "Last name is required.");

    public static readonly Error FirstNameTooLong =
        Error.Validation("User.FirstNameTooLong", "First name must not exceed 100 characters.");

    public static readonly Error LastNameTooLong =
        Error.Validation("User.LastNameTooLong", "Last name must not exceed 100 characters.");

    // =========================
    // Email / Phone Verification
    // =========================

    public static readonly Error EmailAlreadyVerified =
        Error.Conflict("User.EmailAlreadyVerified", "Email is already verified.");

    public static readonly Error PhoneAlreadyVerified =
        Error.Conflict("User.PhoneAlreadyVerified", "Phone number is already verified.");

    // =========================
    // Password
    // =========================

    public static readonly Error SamePassword =
        Error.Conflict("User.SamePassword", "New password must be different from the current password.");

    // =========================
    // Language
    // =========================

    public static readonly Error SameLanguage =
        Error.Conflict("User.SameLanguage", "Selected language is already set.");

    // =========================
    // Profile Image
    // =========================

    public static readonly Error SameProfileImage =
        Error.Conflict("User.SameProfileImage", "Profile image is already the same.");

    // =========================
    // Account Status Transitions
    // =========================

    public static readonly Error AlreadyActive =
        Error.Conflict("User.AlreadyActive", "User is already active.");

    public static readonly Error AlreadySuspended =
        Error.Conflict("User.AlreadySuspended", "User is already suspended.");

    public static readonly Error AlreadyDeactivated =
        Error.Conflict("User.AlreadyDeactivated", "User is already deactivated.");

    public static readonly Error AlreadyDeleted =
        Error.Conflict("User.AlreadyDeleted", "User is already deleted.");
}