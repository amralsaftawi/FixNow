using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Models;
using FixNow.Application.Features.Jobs.Queries.GetCustomerJobEta;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.Technician;

public sealed class TechnicianProfileRepository : ITechnicianProfileRepository
{
    private readonly AppDbContext _dbContext;

    public TechnicianProfileRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianProfiles
            .AsNoTracking()
            .AnyAsync(profile => profile.UserId == userId, cancellationToken);
    }

    public Task<global::TechnicianProfile?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianProfiles
            .Include(profile => profile.Experiences)
            .FirstOrDefaultAsync(
                profile => profile.UserId == userId,
                cancellationToken);
    }

    public Task<global::TechnicianProfile?> GetByUserIdWithServicesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianProfiles
            .Include(profile => profile.Services)
                .ThenInclude(service => service.ServiceCategory)
            .FirstOrDefaultAsync(
                profile => profile.UserId == userId,
                cancellationToken);
    }

    public Task<global::TechnicianProfile?> GetByUserIdWithPortfolioAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianProfiles
            .Include(profile => profile.PortfolioItems)
                .ThenInclude(item => item.Media)
            .FirstOrDefaultAsync(
                profile => profile.UserId == userId,
                cancellationToken);
    }

    public Task<global::TechnicianProfile?> GetByIdWithServicesAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianProfiles
            .Include(profile => profile.Services)
            .FirstOrDefaultAsync(
                profile => profile.Id == id,
                cancellationToken);
    }

    public Task<global::TechnicianProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianProfiles
            .FirstOrDefaultAsync(
                profile => profile.Id == id,
                cancellationToken);
    }

    public Task<TechnicianLocationDto?> GetLocationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianProfiles
            .AsNoTracking()
            .Where(profile => profile.Id == id)
            .Select(profile => new TechnicianLocationDto(
                Latitude: profile.Latitude,
                Longitude: profile.Longitude))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<global::TechnicianProfile>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        VerificationStatus? verificationStatus = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.TechnicianProfiles
            .AsNoTracking()
            .AsQueryable();

        if (verificationStatus is not null)
        {
            query = query.Where(profile =>
                profile.VerificationStatus == verificationStatus.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(profile => profile.CreatedAtUtc)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<global::TechnicianProfile>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }

    public void Update(global::TechnicianProfile technicianProfile)
    {
        _dbContext.TechnicianProfiles.Update(technicianProfile);
    }

    public async Task AddAsync(global::TechnicianProfile technicianProfile, CancellationToken cancellationToken = default)
    {
        await _dbContext.TechnicianProfiles.AddAsync(
            technicianProfile,
            cancellationToken);
    }

    public Task AddExperienceAsync(global::TechnicianExperience experience, CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianExperiences.AddAsync(
            experience,
            cancellationToken).AsTask();
    }

    public void RemoveExperience(global::TechnicianExperience experience)
    {
        _dbContext.TechnicianExperiences.Remove(experience);
    }

    public Task AddPortfolioItemAsync(
        global::TechnicianPortfolioItem portfolioItem,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianPortfolioItems.AddAsync(
            portfolioItem,
            cancellationToken).AsTask();
    }

    public void RemovePortfolioItem(global::TechnicianPortfolioItem portfolioItem)
    {
        _dbContext.TechnicianPortfolioItems.Remove(portfolioItem);
    }

    public Task AddServiceAsync(
        global::TechnicianService service,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianServices.AddAsync(
            service,
            cancellationToken).AsTask();
    }

    public void RemoveService(global::TechnicianService service)
    {
        _dbContext.TechnicianServices.Remove(service);
    }

    public Task<Money?> GetServicePriceByCategoryAsync(
        Guid technicianProfileId,
        Guid serviceCategoryId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.TechnicianServices
            .AsNoTracking()
            .Where(service =>
                service.TechnicianProfileId == technicianProfileId
                && service.ServiceCategoryId == serviceCategoryId)
            .Select(service => service.Price)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
