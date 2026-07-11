
public sealed record TechnicianServiceAddedDomainEvent(
    Guid TechnicianProfileId,
    Guid TechnicianServiceId)
    : DomainEvent;