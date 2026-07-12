public sealed record AssignmentAcceptedDomainEvent(
    Guid AssignmentId,
    Guid ServiceRequestId,
    Guid TechnicianProfileId)
    : DomainEvent;