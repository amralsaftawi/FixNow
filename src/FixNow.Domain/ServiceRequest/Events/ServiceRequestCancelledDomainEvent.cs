
public sealed record ServiceRequestCancelledDomainEvent(
    Guid ServiceRequestId,
    CancellationReason CancellationReason) : DomainEvent;