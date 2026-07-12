public sealed record AssignmentCompletedDomainEvent(
    Guid AssignmentId,
    Guid ServiceRequestId,
    Guid TechnicianProfileId,
    DateTimeOffset CompletedAt)
    : DomainEvent;