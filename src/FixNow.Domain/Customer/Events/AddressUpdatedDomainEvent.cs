
public sealed record AddressUpdatedDomainEvent(
    Guid AddressId,
    Guid CustomerProfileId) : DomainEvent;