public sealed record RefreshTokenRevokedDomainEvent(
    Guid RefreshTokenId,
    Guid UserId,
    DateTimeOffset RevokedAt
) : DomainEvent;