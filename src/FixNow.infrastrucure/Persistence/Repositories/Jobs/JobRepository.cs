using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Models;
using FixNow.Application.Features.Jobs.Queries.GetFinalJobPrice;
using FixNow.Application.Features.Jobs.Queries.GetJobTimeline;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.Jobs;

public sealed class JobRepository(AppDbContext dbContext)
    : IJobRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public Task AddAsync(
        Job job,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Jobs.AddAsync(
            job,
            cancellationToken).AsTask();
    }

    public Task<Job?> GetByServiceRequestIdAsync(
        Guid serviceRequestId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Jobs
            .FirstOrDefaultAsync(
                job => job.ServiceRequestId == serviceRequestId,
                cancellationToken);
    }

    public Task<Job?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Jobs
            .FirstOrDefaultAsync(
                job => job.Id == id,
                cancellationToken);
    }

    public Task<JobAccessDto?> GetAccessAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Lightweight ownership projection used for authorization. The full
        // Job aggregate and its ServiceRequest graph are never loaded.
        return _dbContext.Jobs
            .AsNoTracking()
            .Where(job => job.Id == id)
            .Select(job => new JobAccessDto(
                ServiceRequestId: job.ServiceRequestId,
                ServiceRequestCustomerProfileId: job.ServiceRequest.CustomerProfileId,
                TechnicianProfileId: job.TechnicianProfileId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<JobPricingSourceDto?> GetPricingSourceAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Focused projection of the pricing source used to finalize a job's
        // price at completion: the service category and its current base
        // price and inspection fee. The full Job/ServiceRequest graph is
        // never loaded.
        return _dbContext.Jobs
            .AsNoTracking()
            .Where(job => job.Id == id)
            .Select(job => new JobPricingSourceDto(
                ServiceCategoryId: job.ServiceRequest.ServiceCategoryId,
                BasePrice: job.ServiceRequest.ServiceCategory.Price,
                InspectionFee: job.ServiceRequest.ServiceCategory.InspectionFee))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<JobFinalPriceDto?> GetFinalJobPriceAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Focused projection of every pricing component needed to compute a
        // job's final price: the finalized snapshot (service price and
        // inspection fee captured at completion, when present), the current
        // category pricing, and the sum of the job's additional charges. The
        // full Job aggregate is never materialized.
        return _dbContext.Jobs
            .AsNoTracking()
            .Where(job => job.Id == id)
            .Select(job => new JobFinalPriceDto(
                Status: job.Status,
                FinalizedServicePrice: job.ServicePrice,
                FinalizedInspectionFee: job.InspectionFee,
                ServiceCategoryId: job.ServiceRequest.ServiceCategoryId,
                BasePrice: job.ServiceRequest.ServiceCategory.Price,
                InspectionFee: job.ServiceRequest.ServiceCategory.InspectionFee,
                AdditionalChargesTotal: job.AdditionalCharges.Sum(charge => charge.Amount.Value),
                AdditionalChargesCurrency: job.AdditionalCharges
                    .Select(charge => (Currency?)charge.Amount.Currency)
                    .FirstOrDefault()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<JobTimelineEntryDto>> GetTimelineAsync(
        Guid jobId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.JobTimelines
            .AsNoTracking()
            .Where(entry => entry.JobId == jobId);

        var totalCount = await query.CountAsync(cancellationToken);

        var offset = (pageNumber - 1) * pageSize;

        var items = await query
            .OrderBy(entry => entry.OccurredOn)
            .ThenBy(entry => entry.Id)
            .Skip(offset)
            .Take(pageSize)
            .Select(entry => new JobTimelineEntryDto(
                Id: entry.Id,
                Status: entry.Status,
                Description: entry.Description,
                OccurredOn: entry.OccurredOn))
            .ToListAsync(cancellationToken);

        return new PagedResult<JobTimelineEntryDto>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }

    public void Update(Job job)
    {
        _dbContext.Jobs.Update(job);
    }
}
