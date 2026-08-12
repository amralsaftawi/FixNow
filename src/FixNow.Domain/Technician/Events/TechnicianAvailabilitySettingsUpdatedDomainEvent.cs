
public sealed record TechnicianAvailabilitySettingsUpdatedDomainEvent(
    Guid TechnicianProfileId,
    TechnicianAvailabilityStatus Status)
    : DomainEvent;
