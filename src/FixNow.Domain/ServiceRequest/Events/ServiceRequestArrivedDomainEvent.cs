public sealed record ServiceRequestArrivedDomainEvent(
    Guid ServiceRequestId) : DomainEvent;
