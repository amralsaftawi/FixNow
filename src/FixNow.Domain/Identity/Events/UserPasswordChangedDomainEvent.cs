public sealed record UserPasswordChangedDomainEvent(
    Guid UserId
) : DomainEvent;