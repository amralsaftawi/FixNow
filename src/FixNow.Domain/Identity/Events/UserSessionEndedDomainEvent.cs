
public sealed record UserSessionEndedDomainEvent(
    Guid UserSessionId,
    Guid UserId) : DomainEvent;