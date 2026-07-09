public static class OTPRecordErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "OTPRecord.IdRequired",
            "OTP record id is required.");

    public static readonly Error UserIdRequired =
        Error.Validation(
            "OTPRecord.UserIdRequired",
            "User id is required.");

    public static readonly Error CodeHashRequired =
        Error.Validation(
            "OTPRecord.CodeHashRequired",
            "OTP code hash is required.");

    public static readonly Error InvalidExpirationDate =
        Error.Validation(
            "OTPRecord.InvalidExpirationDate",
            "Expiration date must be in the future.");

    public static readonly Error InvalidMaxAttempts =
        Error.Validation(
            "OTPRecord.InvalidMaxAttempts",
            "Maximum attempts must be greater than zero.");

    public static readonly Error AlreadyVerified =
        Error.Conflict(
            "OTPRecord.AlreadyVerified",
            "This OTP has already been verified.");

    public static readonly Error Expired =
        Error.Conflict(
            "OTPRecord.Expired",
            "This OTP has expired.");

    public static readonly Error MaxAttemptsExceeded =
        Error.Conflict(
            "OTPRecord.MaxAttemptsExceeded",
            "Maximum verification attempts have been exceeded.");
}