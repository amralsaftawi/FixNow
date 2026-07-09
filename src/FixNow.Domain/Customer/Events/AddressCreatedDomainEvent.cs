
public sealed record AddressCreatedDomainEvent(
    Guid AddressId,
    Guid CustomerProfileId) : DomainEvent;