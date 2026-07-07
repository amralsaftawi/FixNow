public sealed record UserActivatedDomainEvent(
    Guid UserId
) : DomainEvent;