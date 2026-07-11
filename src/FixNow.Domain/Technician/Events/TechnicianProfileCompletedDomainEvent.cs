
public sealed record TechnicianProfileCompletedDomainEvent(
    Guid TechnicianProfileId)
    : DomainEvent;