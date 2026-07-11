
public sealed record TechnicianVerificationRejectedDomainEvent(
    Guid TechnicianProfileId,
    Guid UserId)
    : DomainEvent;