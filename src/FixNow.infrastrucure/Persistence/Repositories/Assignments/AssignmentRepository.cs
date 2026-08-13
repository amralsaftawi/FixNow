using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.Assignments;

public sealed class AssignmentRepository(AppDbContext dbContext)
    : IAssignmentRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public Task AddAsync(
        global::Assignment assignment,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Assignments.AddAsync(
            assignment,
            cancellationToken).AsTask();
    }

    public Task<global::Assignment?> GetPendingByRequestAndTechnicianAsync(
        Guid serviceRequestId,
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Assignments
            .FirstOrDefaultAsync(
                assignment =>
                    assignment.ServiceRequestId == serviceRequestId
                    && assignment.TechnicianProfileId == technicianProfileId
                    && assignment.Status == AssignmentStatus.Pending,
                cancellationToken);
    }

    public Task<global::Assignment?> GetPendingByRequestAsync(
        Guid serviceRequestId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Assignments
            .FirstOrDefaultAsync(
                assignment =>
                    assignment.ServiceRequestId == serviceRequestId
                    && assignment.Status == AssignmentStatus.Pending,
                cancellationToken);
    }

    public Task<global::Assignment?> GetAcceptedByRequestAndTechnicianAsync(
        Guid serviceRequestId,
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Assignments
            .FirstOrDefaultAsync(
                assignment =>
                    assignment.ServiceRequestId == serviceRequestId
                    && assignment.TechnicianProfileId == technicianProfileId
                    && assignment.Status == AssignmentStatus.Accepted,
                cancellationToken);
    }
}
