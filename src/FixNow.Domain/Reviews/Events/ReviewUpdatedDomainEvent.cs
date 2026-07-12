public sealed record ReviewUpdatedDomainEvent(
    Guid ReviewId,
    Guid AssignmentId,
    Guid TechnicianProfileId)
    : DomainEvent;