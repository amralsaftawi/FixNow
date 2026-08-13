public sealed record ServiceRequestAcceptedDomainEvent(
    Guid ServiceRequestId) : DomainEvent;
