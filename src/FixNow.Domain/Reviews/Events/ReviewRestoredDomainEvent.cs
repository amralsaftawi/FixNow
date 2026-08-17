public sealed record ReviewRestoredDomainEvent(
    Guid ReviewId,
    Guid AssignmentId,
    Guid TechnicianProfileId)
    : DomainEvent;
