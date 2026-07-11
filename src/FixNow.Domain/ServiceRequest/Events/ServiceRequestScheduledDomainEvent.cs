
public sealed record ServiceRequestScheduledDomainEvent(
    Guid ServiceRequestId,
    DateTimeOffset ScheduledAt) : DomainEvent;