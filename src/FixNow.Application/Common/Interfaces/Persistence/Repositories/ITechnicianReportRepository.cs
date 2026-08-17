namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface ITechnicianReportRepository
{
    Task AddAsync(
        TechnicianReport technicianReport,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByTechnicianAndReporterAsync(
        Guid technicianProfileId,
        Guid reporterUserId,
        CancellationToken cancellationToken = default);
}
