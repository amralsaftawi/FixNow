using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record ReportReviewRequest
{
    [Required]
    public ReviewReportReason Reason { get; init; }

    [MaxLength(1000)]
    public string? Description { get; init; }
}
