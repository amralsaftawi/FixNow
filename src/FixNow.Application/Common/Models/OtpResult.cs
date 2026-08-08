public sealed record OtpResult(
    string Code,
    DateTimeOffset ExpiresAt);