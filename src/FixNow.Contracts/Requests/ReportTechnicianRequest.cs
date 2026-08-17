using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record ReportTechnicianRequest
{
    [Required]
    public TechnicianReportReason Reason { get; init; }

    [MaxLength(1000)]
    public string? Description { get; init; }
}
