using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Authentication;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Reviews.Commands.ReportReview;

public sealed class ReportReviewCommandHandler(
    IReviewRepository reviewRepository,
    IReviewReportRepository reviewReportRepository,
    ICurrentUser currentUser)
    : ICommandHandler<ReportReviewCommand, Result<ReportReviewResponse>>
{
    private readonly IReviewRepository _reviewRepository = reviewRepository;
    private readonly IReviewReportRepository _reviewReportRepository = reviewReportRepository;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<ReportReviewResponse>> Handle(
        ReportReviewCommand command,
        CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(
            command.ReviewId,
            cancellationToken);

        if (review is null)
            return ReviewReportErrors.ReviewNotFound;

        var isOwnReview = await _reviewRepository
            .IsOwnReviewAsync(
                command.ReviewId,
                _currentUser.UserId,
                cancellationToken);

        if (isOwnReview)
            return ReviewReportErrors.CannotReportOwnReview;

        var alreadyReported = await _reviewReportRepository
            .ExistsByReviewAndReporterAsync(
                command.ReviewId,
                _currentUser.UserId,
                cancellationToken);

        if (alreadyReported)
            return ReviewReportErrors.AlreadyReported;

        var reportResult = ReviewReport.Create(
            id: Guid.NewGuid(),
            reviewId: command.ReviewId,
            reporterUserId: _currentUser.UserId,
            reason: command.Reason,
            description: command.Description);

        if (reportResult.IsError)
            return reportResult.Errors;

        await _reviewReportRepository.AddAsync(
            reportResult.Value,
            cancellationToken);

        return new ReportReviewResponse(
            ReviewReportId: reportResult.Value.Id,
            ReviewId: reportResult.Value.ReviewId,
            Reason: reportResult.Value.Reason,
            Status: reportResult.Value.Status,
            CreatedAtUtc: reportResult.Value.CreatedAtUtc);
    }
}
