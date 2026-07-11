
public sealed record TechnicianAvailabilityChangedDomainEvent(
    Guid TechnicianProfileId,
    TechnicianAvailability Availability)
    : DomainEvent;