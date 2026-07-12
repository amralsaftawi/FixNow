public sealed record AssignmentCancelledDomainEvent(
    Guid AssignmentId,
    Guid ServiceRequestId,
    Guid TechnicianProfileId)
    : DomainEvent;