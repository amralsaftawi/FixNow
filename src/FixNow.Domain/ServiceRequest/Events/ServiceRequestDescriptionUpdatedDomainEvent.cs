
public sealed record ServiceRequestDescriptionUpdatedDomainEvent(
    Guid ServiceRequestId) : DomainEvent;