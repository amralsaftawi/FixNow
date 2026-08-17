
public sealed record TechnicianLocationUpdatedDomainEvent(
    Guid TechnicianProfileId,
    decimal Latitude,
    decimal Longitude) : DomainEvent;
