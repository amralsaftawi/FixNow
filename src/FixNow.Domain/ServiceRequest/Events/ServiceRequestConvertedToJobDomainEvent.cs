public sealed record ServiceRequestConvertedToJobDomainEvent(
    Guid ServiceRequestId) : DomainEvent;
