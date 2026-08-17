public sealed record ReviewReportedDomainEvent(
    Guid ReviewReportId,
    Guid ReviewId,
    Guid ReporterUserId)
    : DomainEvent;
