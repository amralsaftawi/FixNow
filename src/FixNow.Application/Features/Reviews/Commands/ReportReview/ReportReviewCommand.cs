using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Reviews.Commands.ReportReview;

public sealed record ReportReviewCommand(
    Guid ReviewId,
    ReviewReportReason Reason,
    string? Description = null)
    : ICommand<Result<ReportReviewResponse>>;
