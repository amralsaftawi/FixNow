public sealed record UserDeactivatedDomainEvent(
    Guid UserId
) : DomainEvent;