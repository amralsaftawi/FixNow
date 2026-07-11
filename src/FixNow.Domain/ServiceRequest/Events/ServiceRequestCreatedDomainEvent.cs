
public sealed record ServiceRequestCreatedDomainEvent(
    Guid ServiceRequestId,
    Guid CustomerProfileId) : DomainEvent;