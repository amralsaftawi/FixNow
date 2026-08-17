public sealed record TechnicianReportedDomainEvent(
    Guid TechnicianReportId,
    Guid TechnicianProfileId,
    Guid ReporterUserId)
    : DomainEvent;
