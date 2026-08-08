
public sealed record CustomerCurrentLocationUpdatedDomainEvent(
    Guid CustomerProfileId,
    decimal Latitude,
    decimal Longitude) : DomainEvent;
