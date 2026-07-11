
public sealed record TechnicianServiceRemovedDomainEvent(
    Guid TechnicianProfileId,
    Guid TechnicianServiceId)
    : DomainEvent;