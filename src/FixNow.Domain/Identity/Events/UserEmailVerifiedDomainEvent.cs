public sealed record UserEmailVerifiedDomainEvent(
    Guid UserId
) : DomainEvent;