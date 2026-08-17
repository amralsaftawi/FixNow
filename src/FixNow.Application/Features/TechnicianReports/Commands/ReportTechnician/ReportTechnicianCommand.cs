using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianReports.Commands.ReportTechnician;

public sealed record ReportTechnicianCommand(
    Guid TechnicianProfileId,
    TechnicianReportReason Reason,
    string? Description = null)
    : ICommand<Result<ReportTechnicianResponse>>;
