
public sealed record ServiceRequestImageRemovedDomainEvent(
    Guid ServiceRequestId,
    Guid ImageId) : DomainEvent;