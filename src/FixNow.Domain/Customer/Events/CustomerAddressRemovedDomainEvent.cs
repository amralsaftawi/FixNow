
public sealed record CustomerAddressRemovedDomainEvent(
    Guid CustomerProfileId,
    Guid AddressId) : DomainEvent;