
public sealed record UserRegisteredDomainEvent(
    Guid UserId
) : DomainEvent;

