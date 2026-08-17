namespace FixNow.Application.Features.TechnicianReports.Commands.ReportTechnician;

public sealed record ReportTechnicianResponse(
    Guid TechnicianReportId,
    Guid TechnicianProfileId,
    TechnicianReportReason Reason,
    TechnicianReportStatus Status,
    DateTimeOffset CreatedAtUtc);
