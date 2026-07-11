

public sealed class UserSession : AuditableEntity
{
    public Guid UserId { get; private set; }

    public Guid RefreshTokenId { get; private set; }

    public string DeviceName { get; private set; }

    public string IpAddress { get; private set; }

    public string UserAgent { get; private set; }

    public DateTimeOffset StartedAt { get; private set; }

    public DateTimeOffset ExpiresAt { get; private set; }

    public DateTimeOffset? EndedAt { get; private set; }

    public bool IsActive { get; private set; }

    // Navigation Properties

    public User User { get; private set; } = null!;

    public RefreshToken RefreshToken { get; private set; } = null!;

#pragma warning disable CS8618
    private UserSession()
    {
    }
    #pragma warning disable CS8618

    private UserSession(
        Guid id,
        Guid userId,
        Guid refreshTokenId,
        string deviceName,
        string ipAddress,
        string userAgent,
        DateTimeOffset expiresAt)
        : base(id)
    {
        UserId = userId;
        RefreshTokenId = refreshTokenId;
        DeviceName = deviceName;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        StartedAt = DateTimeOffset.UtcNow;
        ExpiresAt = expiresAt;
        IsActive = true;
    }

    public static Result<UserSession> Create(
        Guid id,
        Guid userId,
        Guid refreshTokenId,
        string deviceName,
        string ipAddress,
        string userAgent,
        DateTimeOffset expiresAt)
    {
        if (id == Guid.Empty)
            return UserSessionErrors.IdRequired;

        if (userId == Guid.Empty)
            return UserSessionErrors.UserIdRequired;

        if (refreshTokenId == Guid.Empty)
            return UserSessionErrors.RefreshTokenIdRequired;

        if (string.IsNullOrWhiteSpace(deviceName))
            return UserSessionErrors.DeviceNameRequired;

        if (string.IsNullOrWhiteSpace(ipAddress))
            return UserSessionErrors.IpAddressRequired;

        if (string.IsNullOrWhiteSpace(userAgent))
            return UserSessionErrors.UserAgentRequired;

        if (expiresAt <= DateTimeOffset.UtcNow)
            return UserSessionErrors.InvalidExpirationDate;

        var session = new UserSession(
            id,
            userId,
            refreshTokenId,
            deviceName.Trim(),
            ipAddress.Trim(),
            userAgent.Trim(),
            expiresAt);

        session.AddDomainEvent(
            new UserSessionStartedDomainEvent(
                session.Id,
                session.UserId));

        return session;
    }

    public Result<Success> End()
    {
        if (!IsActive)
            return UserSessionErrors.AlreadyEnded;

        IsActive = false;

        EndedAt = DateTimeOffset.UtcNow;

        AddDomainEvent(
            new UserSessionEndedDomainEvent(
                Id,
                UserId));

        return Result.Success;
    }
}