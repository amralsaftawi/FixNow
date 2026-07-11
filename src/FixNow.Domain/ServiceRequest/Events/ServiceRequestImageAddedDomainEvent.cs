
public sealed record ServiceRequestImageAddedDomainEvent(
    Guid ServiceRequestId,
    Guid ImageId) : DomainEvent;