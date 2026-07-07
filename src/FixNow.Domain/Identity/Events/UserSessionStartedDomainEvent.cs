
public sealed record UserSessionStartedDomainEvent(
    Guid UserSessionId,
    Guid UserId) : DomainEvent;