public static class UserErrors
{
    public static readonly Error NotFound =
        Error.NotFound(
            "User.NotFound",
            "The current user was not found.");

    public static readonly Error InvalidAccountStatus =
        Error.Validation("User.AccountStatus.Invalid", "The specified account status is invalid.");
}