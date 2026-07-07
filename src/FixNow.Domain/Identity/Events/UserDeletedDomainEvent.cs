public sealed record UserDeletedDomainEvent(
    Guid UserId,
    DateTimeOffset DeletedAt
) : DomainEvent;