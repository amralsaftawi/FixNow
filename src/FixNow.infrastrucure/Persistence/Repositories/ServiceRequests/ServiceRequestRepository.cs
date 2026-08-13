using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Models;
using FixNow.Application.Features.TechnicianRequests.Queries.GetAvailableServiceRequests;
using FixNow.Application.Features.TechnicianRequests.Queries.GetServiceRequestDetails;
using FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianActiveJobs;
using FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianJobHistory;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.ServiceRequests;

public sealed class ServiceRequestRepository(AppDbContext dbContext)
    : IServiceRequestRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task AddAsync(
        ServiceRequest serviceRequest,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.ServiceRequests.AddAsync(
            serviceRequest,
            cancellationToken);
    }

    public Task<ServiceRequest?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.ServiceRequests
            .FirstOrDefaultAsync(
                serviceRequest => serviceRequest.Id == id,
                cancellationToken);
    }

    public void Update(ServiceRequest serviceRequest)
    {
        _dbContext.ServiceRequests.Update(serviceRequest);
    }

    public Task AddImageAsync(
        ServiceRequestImage image,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.ServiceRequestImages.AddAsync(
            image,
            cancellationToken).AsTask();
    }

    public async Task<PagedResult<AvailableServiceRequestDto>> GetAvailableForTechnicianAsync(
        IReadOnlyCollection<Guid> serviceCategoryIds,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.ServiceRequests
            .AsNoTracking()
            .Where(serviceRequest =>
                serviceRequest.Status == ServiceRequestStatus.SearchingTechnician
                && serviceCategoryIds.Contains(serviceRequest.ServiceCategoryId));

        var totalCount = await query.CountAsync(cancellationToken);

        var offset = (pageNumber - 1) * pageSize;

        var items = await query
            .OrderByDescending(serviceRequest => serviceRequest.RequestedAt)
            .Skip(offset)
            .Take(pageSize)
            .Select(serviceRequest => new AvailableServiceRequestDto(
                ServiceRequestId: serviceRequest.Id,
                ServiceCategoryId: serviceRequest.ServiceCategoryId,
                ServiceCategoryName: serviceRequest.ServiceCategory.Name,
                ProblemTypeId: serviceRequest.ProblemTypeId,
                ProblemTypeName: serviceRequest.ProblemType == null
                    ? null
                    : serviceRequest.ProblemType.Name,
                Description: serviceRequest.Description,
                Priority: serviceRequest.Priority,
                RequestedAt: serviceRequest.RequestedAt,
                ScheduledAt: serviceRequest.ScheduledAt,
                EstimatedCost: serviceRequest.EstimatedCost,
                FullAddress: serviceRequest.Address.FullAddress,
                Latitude: serviceRequest.Address.Latitude,
                Longitude: serviceRequest.Address.Longitude))
            .ToListAsync(cancellationToken);

        return new PagedResult<AvailableServiceRequestDto>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }

    public Task<GetServiceRequestDetailsDto?> GetDetailsForTechnicianAsync(
        Guid serviceRequestId,
        IReadOnlyCollection<Guid> serviceCategoryIds,
        CancellationToken cancellationToken = default)
    {
        // The technician's data scope is applied inside the query itself: the
        // request must exist, must be currently searching for a technician,
        // and must belong to one of the technician's service categories. A
        // request outside this scope returns the same null result as a
        // non-existent request, preventing IDOR/BOLA-style probing.
        return _dbContext.ServiceRequests
            .AsNoTracking()
            .Where(serviceRequest =>
                serviceRequest.Id == serviceRequestId
                && serviceRequest.Status == ServiceRequestStatus.SearchingTechnician
                && serviceCategoryIds.Contains(serviceRequest.ServiceCategoryId))
            .Select(serviceRequest => new GetServiceRequestDetailsDto(
                ServiceRequestId: serviceRequest.Id,
                ServiceCategoryId: serviceRequest.ServiceCategoryId,
                ServiceCategoryName: serviceRequest.ServiceCategory.Name,
                ProblemTypeId: serviceRequest.ProblemTypeId,
                ProblemTypeName: serviceRequest.ProblemType == null
                    ? null
                    : serviceRequest.ProblemType.Name,
                Description: serviceRequest.Description,
                Priority: serviceRequest.Priority,
                Status: serviceRequest.Status,
                RequestedAt: serviceRequest.RequestedAt,
                ScheduledAt: serviceRequest.ScheduledAt,
                EstimatedCost: serviceRequest.EstimatedCost,
                FullAddress: serviceRequest.Address.FullAddress,
                Latitude: serviceRequest.Address.Latitude,
                Longitude: serviceRequest.Address.Longitude,
                ImageKeys: serviceRequest.Images
                    .Select(image => image.ImageKey)
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<ActiveServiceRequestDto>> GetActiveJobsForTechnicianAsync(
        Guid technicianProfileId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        // A request is an active job for a technician only when it is in a
        // working state AND the technician holds an accepted assignment for
        // it. Requiring the assignment to be Accepted (rather than merely
        // existing) is important: if this technician previously rejected the
        // request and another technician accepted it, the technician must
        // never see it again.
        var query = _dbContext.ServiceRequests
            .AsNoTracking()
            .Where(serviceRequest =>
                serviceRequest.Status == ServiceRequestStatus.Accepted
                || serviceRequest.Status == ServiceRequestStatus.InProgress)
            .Where(serviceRequest => _dbContext.Assignments.Any(assignment =>
                assignment.ServiceRequestId == serviceRequest.Id
                && assignment.TechnicianProfileId == technicianProfileId
                && assignment.Status == AssignmentStatus.Accepted));

        var totalCount = await query.CountAsync(cancellationToken);

        var offset = (pageNumber - 1) * pageSize;

        var items = await query
            .OrderByDescending(serviceRequest => serviceRequest.RequestedAt)
            .Skip(offset)
            .Take(pageSize)
            .Select(serviceRequest => new ActiveServiceRequestDto(
                ServiceRequestId: serviceRequest.Id,
                ServiceCategoryId: serviceRequest.ServiceCategoryId,
                ServiceCategoryName: serviceRequest.ServiceCategory.Name,
                ProblemTypeId: serviceRequest.ProblemTypeId,
                ProblemTypeName: serviceRequest.ProblemType == null
                    ? null
                    : serviceRequest.ProblemType.Name,
                Description: serviceRequest.Description,
                Priority: serviceRequest.Priority,
                Status: serviceRequest.Status,
                RequestedAt: serviceRequest.RequestedAt,
                ScheduledAt: serviceRequest.ScheduledAt,
                EstimatedCost: serviceRequest.EstimatedCost,
                FullAddress: serviceRequest.Address.FullAddress,
                Latitude: serviceRequest.Address.Latitude,
                Longitude: serviceRequest.Address.Longitude))
            .ToListAsync(cancellationToken);

        return new PagedResult<ActiveServiceRequestDto>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }

    public async Task<PagedResult<HistoricalServiceRequestDto>> GetJobHistoryForTechnicianAsync(
        Guid technicianProfileId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        // A request belongs to a technician's history only when it has
        // reached a terminal (non-active) state AND the technician holds an
        // accepted or completed assignment on it. Rejected assignments never
        // establish ownership: a request rejected by this technician and
        // later accepted by another must not appear in this technician's
        // history. Requests cancelled before acceptance have no accepted
        // assignment, so they are also excluded.
        var query = _dbContext.ServiceRequests
            .AsNoTracking()
            .Where(serviceRequest =>
                serviceRequest.Status == ServiceRequestStatus.Completed
                || serviceRequest.Status == ServiceRequestStatus.Cancelled)
            .Where(serviceRequest => _dbContext.Assignments.Any(assignment =>
                assignment.ServiceRequestId == serviceRequest.Id
                && assignment.TechnicianProfileId == technicianProfileId
                && (assignment.Status == AssignmentStatus.Accepted
                    || assignment.Status == AssignmentStatus.Completed)));

        var totalCount = await query.CountAsync(cancellationToken);

        var offset = (pageNumber - 1) * pageSize;

        var items = await query
            .OrderByDescending(serviceRequest =>
                serviceRequest.CompletedAt ?? serviceRequest.CancelledAt)
            .ThenByDescending(serviceRequest => serviceRequest.RequestedAt)
            .Skip(offset)
            .Take(pageSize)
            .Select(serviceRequest => new HistoricalServiceRequestDto(
                ServiceRequestId: serviceRequest.Id,
                ServiceCategoryId: serviceRequest.ServiceCategoryId,
                ServiceCategoryName: serviceRequest.ServiceCategory.Name,
                ProblemTypeId: serviceRequest.ProblemTypeId,
                ProblemTypeName: serviceRequest.ProblemType == null
                    ? null
                    : serviceRequest.ProblemType.Name,
                Description: serviceRequest.Description,
                Priority: serviceRequest.Priority,
                Status: serviceRequest.Status,
                RequestedAt: serviceRequest.RequestedAt,
                ScheduledAt: serviceRequest.ScheduledAt,
                EstimatedCost: serviceRequest.EstimatedCost,
                FullAddress: serviceRequest.Address.FullAddress,
                Latitude: serviceRequest.Address.Latitude,
                Longitude: serviceRequest.Address.Longitude))
            .ToListAsync(cancellationToken);

        return new PagedResult<HistoricalServiceRequestDto>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }
}
