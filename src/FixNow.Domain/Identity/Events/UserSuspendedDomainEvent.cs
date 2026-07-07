public sealed record UserSuspendedDomainEvent(
    Guid UserId
) : DomainEvent;