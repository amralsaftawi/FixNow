namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

using FixNow.Application.Common.Models;
using FixNow.Application.Features.Jobs.Queries.GetFinalJobPrice;
using FixNow.Application.Features.Jobs.Queries.GetJobTimeline;

public interface IJobRepository
{
    Task AddAsync(
        Job job,
        CancellationToken cancellationToken = default);

    Task<Job?> GetByServiceRequestIdAsync(
        Guid serviceRequestId,
        CancellationToken cancellationToken = default);

    Task<Job?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<JobAccessDto?> GetAccessAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<JobPricingSourceDto?> GetPricingSourceAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<JobFinalPriceDto?> GetFinalJobPriceAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<JobTimelineEntryDto>> GetTimelineAsync(
        Guid jobId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    void Update(Job job);
}
