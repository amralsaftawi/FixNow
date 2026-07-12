public sealed record ReviewCreatedDomainEvent(
    Guid ReviewId,
    Guid AssignmentId,
    Guid TechnicianProfileId)
    : DomainEvent;