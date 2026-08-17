
public sealed record JobStatusChangedDomainEvent(
    Guid JobId,
    Guid ServiceRequestId,
    Guid TechnicianProfileId,
    JobStatus PreviousStatus,
    JobStatus NewStatus) : DomainEvent;
