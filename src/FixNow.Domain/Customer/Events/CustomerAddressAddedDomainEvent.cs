
public sealed record CustomerAddressAddedDomainEvent(
    Guid CustomerProfileId,
    Guid AddressId) : DomainEvent;