using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Authentication;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianReports.Commands.ReportTechnician;

public sealed class ReportTechnicianCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ITechnicianReportRepository technicianReportRepository,
    ICurrentUser currentUser)
    : ICommandHandler<ReportTechnicianCommand, Result<ReportTechnicianResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository = technicianProfileRepository;
    private readonly ITechnicianReportRepository _technicianReportRepository = technicianReportRepository;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<ReportTechnicianResponse>> Handle(
        ReportTechnicianCommand command,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await _technicianProfileRepository.GetByIdAsync(
            command.TechnicianProfileId,
            cancellationToken);

        if (technicianProfile is null)
            return TechnicianReportErrors.TechnicianNotFound;

        if (technicianProfile.UserId == _currentUser.UserId)
            return TechnicianReportErrors.CannotReportSelf;

        var alreadyReported = await _technicianReportRepository
            .ExistsByTechnicianAndReporterAsync(
                command.TechnicianProfileId,
                _currentUser.UserId,
                cancellationToken);

        if (alreadyReported)
            return TechnicianReportErrors.AlreadyReported;

        var reportResult = TechnicianReport.Create(
            id: Guid.NewGuid(),
            technicianProfileId: command.TechnicianProfileId,
            reporterUserId: _currentUser.UserId,
            reason: command.Reason,
            description: command.Description);

        if (reportResult.IsError)
            return reportResult.Errors;

        await _technicianReportRepository.AddAsync(
            reportResult.Value,
            cancellationToken);

        return new ReportTechnicianResponse(
            TechnicianReportId: reportResult.Value.Id,
            TechnicianProfileId: reportResult.Value.TechnicianProfileId,
            Reason: reportResult.Value.Reason,
            Status: reportResult.Value.Status,
            CreatedAtUtc: reportResult.Value.CreatedAtUtc);
    }
}
