public sealed record ServiceRequestEstimatedCostChangedDomainEvent(
    Guid ServiceRequestId,
    decimal Amount,
    Currency Currency)
    : DomainEvent;
