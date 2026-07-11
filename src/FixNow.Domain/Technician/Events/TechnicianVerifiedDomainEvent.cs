
public sealed record TechnicianVerifiedDomainEvent(
    Guid TechnicianProfileId,
    Guid UserId)
    : DomainEvent;