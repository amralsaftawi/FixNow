
public sealed record ServiceRequestCompletedDomainEvent(
    Guid ServiceRequestId,
    DateTimeOffset CompletedAt) : DomainEvent;