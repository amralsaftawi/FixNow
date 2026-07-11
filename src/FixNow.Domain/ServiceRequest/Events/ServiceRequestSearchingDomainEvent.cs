
public sealed record ServiceRequestSearchingDomainEvent(
    Guid ServiceRequestId) : DomainEvent;