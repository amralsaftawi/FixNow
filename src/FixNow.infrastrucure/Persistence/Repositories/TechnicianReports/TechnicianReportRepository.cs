using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.TechnicianReports;

public sealed class TechnicianReportRepository(AppDbContext dbContext) : ITechnicianReportRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public Task AddAsync(
        TechnicianReport technicianReport,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianReports.AddAsync(
            technicianReport,
            cancellationToken).AsTask();
    }

    public Task<bool> ExistsByTechnicianAndReporterAsync(
        Guid technicianProfileId,
        Guid reporterUserId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianReports
            .AsNoTracking()
            .AnyAsync(
                report =>
                    report.TechnicianProfileId == technicianProfileId
                    && report.ReporterUserId == reporterUserId,
                cancellationToken);
    }
}
