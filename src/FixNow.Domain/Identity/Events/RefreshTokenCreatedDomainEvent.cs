public sealed record RefreshTokenCreatedDomainEvent(
    Guid RefreshTokenId,
    Guid UserId,
    DateTimeOffset ExpiresAt
) : DomainEvent;