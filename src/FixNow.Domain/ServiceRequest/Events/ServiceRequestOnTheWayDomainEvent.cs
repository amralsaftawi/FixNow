public sealed record ServiceRequestOnTheWayDomainEvent(
    Guid ServiceRequestId) : DomainEvent;
