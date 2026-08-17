using FixNow.Application.Common.Models;
using FixNow.Application.Features.Jobs.Queries.GetCustomerJobEta;
using FixNow.Application.Features.ServiceRequests.Queries.GetBaseServicePrice;
using FixNow.Application.Features.TechnicianRequests.Queries.GetAvailableServiceRequests;
using FixNow.Application.Features.TechnicianRequests.Queries.GetServiceRequestDetails;
using FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianActiveJobs;
using FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianJobHistory;

namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface IServiceRequestRepository
{
    Task AddAsync(
        ServiceRequest serviceRequest,
        CancellationToken cancellationToken = default);

    Task<ServiceRequest?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<ServiceRequestDestinationDto?> GetDestinationForCustomerAsync(
        Guid serviceRequestId,
        Guid customerProfileId,
        CancellationToken cancellationToken = default);

    Task<ServiceRequestBasePriceDto?> GetBaseServicePriceAsync(
        Guid serviceRequestId,
        Guid customerProfileId,
        CancellationToken cancellationToken = default);

    void Update(ServiceRequest serviceRequest);

    Task AddImageAsync(
        ServiceRequestImage image,
        CancellationToken cancellationToken = default);

    Task<PagedResult<AvailableServiceRequestDto>> GetAvailableForTechnicianAsync(
        IReadOnlyCollection<Guid> serviceCategoryIds,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<GetServiceRequestDetailsDto?> GetDetailsForTechnicianAsync(
        Guid serviceRequestId,
        IReadOnlyCollection<Guid> serviceCategoryIds,
        CancellationToken cancellationToken = default);

    Task<PagedResult<ActiveServiceRequestDto>> GetActiveJobsForTechnicianAsync(
        Guid technicianProfileId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<PagedResult<HistoricalServiceRequestDto>> GetJobHistoryForTechnicianAsync(
        Guid technicianProfileId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
