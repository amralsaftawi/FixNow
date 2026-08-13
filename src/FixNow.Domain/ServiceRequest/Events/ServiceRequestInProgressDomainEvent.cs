public sealed record ServiceRequestInProgressDomainEvent(
    Guid ServiceRequestId) : DomainEvent;
