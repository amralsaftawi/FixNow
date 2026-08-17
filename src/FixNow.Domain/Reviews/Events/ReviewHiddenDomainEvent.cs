public sealed record ReviewHiddenDomainEvent(
    Guid ReviewId,
    Guid AssignmentId,
    Guid TechnicianProfileId)
    : DomainEvent;
