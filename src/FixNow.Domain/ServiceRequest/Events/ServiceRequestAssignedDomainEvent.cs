
public sealed record ServiceRequestAssignedDomainEvent(
    Guid ServiceRequestId) : DomainEvent;
