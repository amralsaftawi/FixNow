
public sealed record ServiceRequestUnassignedDomainEvent(
    Guid ServiceRequestId) : DomainEvent;
