
public sealed record TechnicianProfileCreatedDomainEvent(
    Guid TechnicianProfileId,
    Guid UserId)
    : DomainEvent;