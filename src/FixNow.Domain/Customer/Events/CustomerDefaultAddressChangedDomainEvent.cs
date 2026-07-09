
public sealed record CustomerDefaultAddressChangedDomainEvent(
    Guid CustomerProfileId,
    Guid AddressId) : DomainEvent;