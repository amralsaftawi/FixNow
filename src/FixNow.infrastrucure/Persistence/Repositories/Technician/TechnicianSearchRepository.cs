using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Models;
using FixNow.Application.Features.TechnicianSearch.Queries.SearchTechnicians;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.Technician;

public sealed class TechnicianSearchRepository : ITechnicianSearchRepository
{
    private readonly AppDbContext _dbContext;

    public TechnicianSearchRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<TechnicianSearchResultDto>> SearchAsync(
        string searchTerm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var pattern = $"%{searchTerm}%";

        var query = _dbContext.TechnicianProfiles
            .AsNoTracking()
            .Where(profile =>
                profile.User.AccountStatus == AccountStatus.Active
                && profile.VerificationStatus == VerificationStatus.Verified
                && EF.Functions.ILike(
                    profile.User.FirstName + " " + profile.User.LastName,
                    pattern));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(profile => profile.User.FirstName)
            .ThenBy(profile => profile.User.LastName)
            .Select(profile => new TechnicianSearchResultDto(
                TechnicianProfileId: profile.Id,
                FirstName: profile.User.FirstName,
                LastName: profile.User.LastName,
                ProfileImageKey: profile.User.ProfileImageKey,
                Bio: profile.Bio,
                YearsOfExperience: profile.YearsOfExperience))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TechnicianSearchResultDto>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }
}
