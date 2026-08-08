namespace FixNow.Contracts.Requests;

public sealed record ResetPasswordRequest(
    string Identifier,
    string Otp,
    string NewPassword,
    string ConfirmPassword);