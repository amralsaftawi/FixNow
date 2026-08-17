using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.ReviewReports;

public sealed class ReviewReportRepository(AppDbContext dbContext) : IReviewReportRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public Task AddAsync(
        ReviewReport reviewReport,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.ReviewReports.AddAsync(
            reviewReport,
            cancellationToken).AsTask();
    }

    public Task<bool> ExistsByReviewAndReporterAsync(
        Guid reviewId,
        Guid reporterUserId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.ReviewReports
            .AsNoTracking()
            .AnyAsync(
                report =>
                    report.ReviewId == reviewId
                    && report.ReporterUserId == reporterUserId,
                cancellationToken);
    }
}
