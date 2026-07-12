public sealed record AssignmentCreatedDomainEvent(
    Guid AssignmentId,
    Guid ServiceRequestId,
    Guid TechnicianProfileId)
    : DomainEvent;