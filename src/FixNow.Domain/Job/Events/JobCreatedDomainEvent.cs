
public sealed record JobCreatedDomainEvent(
    Guid JobId,
    Guid ServiceRequestId,
    Guid TechnicianProfileId) : DomainEvent;
