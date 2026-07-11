
public sealed record ServiceRequestPriorityChangedDomainEvent(
    Guid ServiceRequestId,
    ServicePriority Priority) : DomainEvent;