
public sealed record AddressMarkedAsDefaultDomainEvent(
    Guid AddressId,
    Guid CustomerProfileId) : DomainEvent;