namespace FixNow.Application.Features.Reviews.Commands.ReportReview;

public sealed record ReportReviewResponse(
    Guid ReviewReportId,
    Guid ReviewId,
    ReviewReportReason Reason,
    ReviewReportStatus Status,
    DateTimeOffset CreatedAtUtc);
