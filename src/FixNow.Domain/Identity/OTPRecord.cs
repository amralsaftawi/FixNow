public sealed class OTPRecord : AuditableEntity
{
    public Guid UserId { get; private set; }

    public string CodeHash { get; private set; }

    public OtpPurpose Purpose { get; private set; }

    public DateTimeOffset ExpiresAt { get; private set; }

    public DateTimeOffset? VerifiedAt { get; private set; }

    public int AttemptsCount { get; private set; }

    public int MaxAttempts { get; private set; }

    // Navigation Property

    public User User { get; private set; } = null!;

#pragma warning disable CS8618
    private OTPRecord()
    {
    }
#pragma warning disable CS8618
    private OTPRecord(
        Guid id,
        Guid userId,
        string codeHash,
        OtpPurpose purpose,
        DateTimeOffset expiresAt,
        int maxAttempts)
        : base(id)
    {
        UserId = userId;
        CodeHash = codeHash;
        Purpose = purpose;
        ExpiresAt = expiresAt;
        MaxAttempts = maxAttempts;
        AttemptsCount = 0;
    }

    public static Result<OTPRecord> Create(
        Guid id,
        Guid userId,
        string codeHash,
        OtpPurpose purpose,
        DateTimeOffset expiresAt,
        int maxAttempts)
    {
        if (id == Guid.Empty)
            return OTPRecordErrors.IdRequired;

        if (userId == Guid.Empty)
            return OTPRecordErrors.UserIdRequired;

        if (string.IsNullOrWhiteSpace(codeHash))
            return OTPRecordErrors.CodeHashRequired;

        if (expiresAt <= DateTimeOffset.UtcNow)
            return OTPRecordErrors.InvalidExpirationDate;

        if (maxAttempts <= 0)
            return OTPRecordErrors.InvalidMaxAttempts;

        var otpRecord = new OTPRecord(
            id,
            userId,
            codeHash.Trim(),
            purpose,
            expiresAt,
            maxAttempts);

        otpRecord.AddDomainEvent(
            new OtpCreatedDomainEvent(
                otpRecord.Id,
                otpRecord.UserId,
                otpRecord.Purpose));

        return otpRecord;
    }

    public Result<Success> IncrementAttempts()
    {
        if (IsVerified)
            return OTPRecordErrors.AlreadyVerified;

        if (IsExpired)
            return OTPRecordErrors.Expired;

        if (IsLocked)
            return OTPRecordErrors.MaxAttemptsExceeded;

        AttemptsCount++;

        return Result.Success;
    }

    public Result<Success> Verify()
    {
        if (IsVerified)
            return OTPRecordErrors.AlreadyVerified;

        if (IsLocked)
            return OTPRecordErrors.MaxAttemptsExceeded;

        if (IsExpired)
            return OTPRecordErrors.Expired;

        VerifiedAt = DateTimeOffset.UtcNow;

        AddDomainEvent(
            new OtpVerifiedDomainEvent(
                Id,
                UserId,
                Purpose));

        return Result.Success;
    }

    public bool IsExpired =>
        DateTimeOffset.UtcNow > ExpiresAt;

    public bool IsVerified =>
        VerifiedAt.HasValue;

    public bool IsLocked =>
        AttemptsCount >= MaxAttempts;

    public bool CanRetry =>
        !IsVerified &&
        !IsExpired &&
        !IsLocked;
}