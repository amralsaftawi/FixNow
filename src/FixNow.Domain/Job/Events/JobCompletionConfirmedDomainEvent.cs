
public sealed record JobCompletionConfirmedDomainEvent(
    Guid JobId,
    Guid ServiceRequestId,
    Guid TechnicianProfileId,
    DateTimeOffset ConfirmedAtUtc) : DomainEvent;
