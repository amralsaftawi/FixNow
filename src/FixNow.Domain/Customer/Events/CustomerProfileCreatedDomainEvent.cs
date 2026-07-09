
public sealed record CustomerProfileCreatedDomainEvent(
    Guid CustomerProfileId,
    Guid UserId) : DomainEvent;