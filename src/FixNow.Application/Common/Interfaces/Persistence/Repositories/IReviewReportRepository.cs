namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface IReviewReportRepository
{
    Task AddAsync(
        ReviewReport reviewReport,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByReviewAndReporterAsync(
        Guid reviewId,
        Guid reporterUserId,
        CancellationToken cancellationToken = default);
}
