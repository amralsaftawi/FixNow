
public sealed record TechnicianVerificationSubmittedDomainEvent(
    Guid TechnicianProfileId,
    Guid UserId)
    : DomainEvent;
