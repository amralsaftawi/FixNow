using FixNow.Domain.Common.Errors;

public static class StorageErrors
{
    public static readonly Error StoreFailed =
        Error.Failure(
            code: "Storage.StoreFailed",
            description: "The file could not be stored.");

    public static readonly Error DeleteFailed =
        Error.Failure(
            code: "Storage.DeleteFailed",
            description: "The file could not be deleted.");

    public static readonly Error InvalidKey =
        Error.Validation(
            code: "Storage.InvalidKey",
            description: "The file key is invalid.");
}
