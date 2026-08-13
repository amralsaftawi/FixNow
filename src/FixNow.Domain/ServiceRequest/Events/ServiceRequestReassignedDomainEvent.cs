
public sealed record ServiceRequestReassignedDomainEvent(
    Guid ServiceRequestId) : DomainEvent;
