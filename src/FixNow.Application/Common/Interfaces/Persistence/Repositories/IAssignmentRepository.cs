namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface IAssignmentRepository
{
    Task AddAsync(
        Assignment assignment,
        CancellationToken cancellationToken = default);

    Task<Assignment?> GetPendingByRequestAndTechnicianAsync(
        Guid serviceRequestId,
        Guid technicianProfileId,
        CancellationToken cancellationToken = default);

    Task<Assignment?> GetPendingByRequestAsync(
        Guid serviceRequestId,
        CancellationToken cancellationToken = default);

    Task<Assignment?> GetAcceptedByRequestAndTechnicianAsync(
        Guid serviceRequestId,
        Guid technicianProfileId,
        CancellationToken cancellationToken = default);
}
