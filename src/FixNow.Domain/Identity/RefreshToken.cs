public sealed class RefreshToken : AuditableEntity
{
    public Guid UserId { get; private set; }

    public string RefreshTokenHash { get; private set; }

    public DateTimeOffset ExpiresAt { get; private set; }

    public DateTimeOffset? RevokedAt { get; private set; }

    public bool IsRevoked { get; private set; }

    // Navigation

    public User User { get; private set; } = null!;

#pragma warning disable CS8618
    private RefreshToken()
    {
    }
#pragma warning disable CS8618
    private RefreshToken(
        Guid id,
        Guid userId,
        string token,
        DateTimeOffset expiresAt)
        : base(id)
    {
        UserId = userId;
        RefreshTokenHash = token;
        ExpiresAt = expiresAt;
        IsRevoked = false;
    }

    public static Result<RefreshToken> Create(
        Guid id,
        Guid userId,
        string token,
        DateTimeOffset expiresAt)
    {
        if (id == Guid.Empty)
            return RefreshTokenErrors.IdRequired;

        if (userId == Guid.Empty)
            return RefreshTokenErrors.UserIdRequired;

        if (string.IsNullOrWhiteSpace(token))
            return RefreshTokenErrors.TokenRequired;

        if (expiresAt <= DateTimeOffset.UtcNow)
            return RefreshTokenErrors.InvalidExpirationDate;

        var refreshToken = new RefreshToken(
            id,
            userId,
            token.Trim(),
            expiresAt);

        refreshToken.AddDomainEvent(
            new RefreshTokenCreatedDomainEvent(
                refreshToken.Id,
                refreshToken.UserId,
                refreshToken.ExpiresAt));

        return refreshToken;
    }


   public bool IsExpired =>
    ExpiresAt <= DateTimeOffset.UtcNow;

   public Result<Success> Revoke()
{
    if (IsRevoked)
        return RefreshTokenErrors.AlreadyRevoked;

    if (IsExpired)
        return RefreshTokenErrors.AlreadyExpired;

    IsRevoked = true;
    RevokedAt = DateTimeOffset.UtcNow;

    AddDomainEvent(
        new RefreshTokenRevokedDomainEvent(
            Id,
            UserId,
            RevokedAt.Value));

    return Result.Success;
}
}