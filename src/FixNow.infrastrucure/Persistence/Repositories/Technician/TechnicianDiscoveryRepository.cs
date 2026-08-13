using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Models;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByLocation;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByRating;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByService;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianServices;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.Technician;

public sealed class TechnicianDiscoveryRepository : ITechnicianDiscoveryRepository
{
    private const double EarthRadiusKm = 6371.0;

    private static readonly double KilometersPerDegree = 111.32;

    private readonly AppDbContext _dbContext;

    public TechnicianDiscoveryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<NearbyTechnicianDto>> FindNearbyAsync(
        decimal latitude,
        decimal longitude,
        double radiusInKm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var (minLatitude, maxLatitude, minLongitude, maxLongitude) =
            BoundingBox(latitude, longitude, radiusInKm);

        var offset = (pageNumber - 1) * pageSize;

        var rows = await _dbContext.Database
            .SqlQuery<TechnicianDiscoveryProjection>(
                $"""
                SELECT
                    t."Id" AS "TechnicianProfileId",
                    t."UserId" AS "UserId",
                    u."FirstName" AS "FirstName",
                    u."LastName" AS "LastName",
                    u."ProfileImageKey" AS "ProfileImageKey",
                    t."Bio" AS "Bio",
                    t."YearsOfExperience" AS "YearsOfExperience",
                    t."Latitude" AS "Latitude",
                    t."Longitude" AS "Longitude",
                    ({EarthRadiusKm} * acos(least(1.0, greatest(-1.0,
                        cos(radians({latitude})) * cos(radians(t."Latitude")) *
                        cos(radians(t."Longitude") - radians({longitude})) +
                        sin(radians({latitude})) * sin(radians(t."Latitude"))
                    )))) AS "DistanceInKm",
                    COUNT(*) OVER () AS "TotalCount"
                FROM "TechnicianProfiles" AS t
                JOIN "Users" AS u ON u."Id" = t."UserId"
                WHERE u."AccountStatus" = {(int)AccountStatus.Active}
                  AND t."VerificationStatus" = {(int)VerificationStatus.Verified}
                  AND t."Latitude" IS NOT NULL
                  AND t."Longitude" IS NOT NULL
                  AND t."Latitude" BETWEEN {minLatitude} AND {maxLatitude}
                  AND t."Longitude" BETWEEN {minLongitude} AND {maxLongitude}
                  AND ({EarthRadiusKm} * acos(least(1.0, greatest(-1.0,
                        cos(radians({latitude})) * cos(radians(t."Latitude")) *
                        cos(radians(t."Longitude") - radians({longitude})) +
                        sin(radians({latitude})) * sin(radians(t."Latitude"))
                    )))) <= {radiusInKm}
                ORDER BY "DistanceInKm"
                LIMIT {pageSize} OFFSET {offset}
                """)
            .ToListAsync(cancellationToken);

        var items = rows
            .Select(ToDto)
            .ToList();

        var totalCount = rows.Count == 0
            ? 0
            : (int)rows[0].TotalCount;

        return new PagedResult<NearbyTechnicianDto>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }

    public async Task<PagedResult<ServiceTechnicianDto>> GetByServiceCategoryAsync(
        Guid serviceCategoryId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.TechnicianProfiles
            .AsNoTracking()
            .Where(profile =>
                profile.User.AccountStatus == AccountStatus.Active
                && profile.VerificationStatus == VerificationStatus.Verified
                && profile.Services.Any(service =>
                    service.ServiceCategoryId == serviceCategoryId
                    && service.ServiceCategory.IsActive));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(profile => profile.User.FirstName)
            .ThenBy(profile => profile.User.LastName)
            .Select(profile => new ServiceTechnicianDto(
                TechnicianProfileId: profile.Id,
                FirstName: profile.User.FirstName,
                LastName: profile.User.LastName,
                ProfileImageKey: profile.User.ProfileImageKey,
                Bio: profile.Bio,
                YearsOfExperience: profile.YearsOfExperience))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ServiceTechnicianDto>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }

    public async Task<PagedResult<LocatedTechnicianDto>> GetByCityAsync(
        int cityId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.TechnicianProfiles
            .AsNoTracking()
            .Where(profile =>
                profile.CityId == cityId
                && profile.User.AccountStatus == AccountStatus.Active
                && profile.VerificationStatus == VerificationStatus.Verified);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(profile => profile.User.FirstName)
            .ThenBy(profile => profile.User.LastName)
            .Select(profile => new LocatedTechnicianDto(
                TechnicianProfileId: profile.Id,
                FirstName: profile.User.FirstName,
                LastName: profile.User.LastName,
                ProfileImageKey: profile.User.ProfileImageKey,
                Bio: profile.Bio,
                YearsOfExperience: profile.YearsOfExperience))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<LocatedTechnicianDto>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }

    public async Task<PagedResult<RatedTechnicianDto>> GetByMinimumRatingAsync(
        double minimumRating,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var offset = (pageNumber - 1) * pageSize;

        var rows = await _dbContext.Database
            .SqlQuery<RatedTechnicianProjection>(
                $"""
                SELECT
                    t."Id" AS "TechnicianProfileId",
                    u."FirstName" AS "FirstName",
                    u."LastName" AS "LastName",
                    u."ProfileImageKey" AS "ProfileImageKey",
                    t."Bio" AS "Bio",
                    t."YearsOfExperience" AS "YearsOfExperience",
                    AVG(r."Rating")::double precision AS "AverageRating",
                    COUNT(*) OVER () AS "TotalCount"
                FROM "TechnicianProfiles" AS t
                JOIN "Users" AS u ON u."Id" = t."UserId"
                JOIN "Reviews" AS r ON r."TechnicianProfileId" = t."Id"
                WHERE u."AccountStatus" = {(int)AccountStatus.Active}
                  AND t."VerificationStatus" = {(int)VerificationStatus.Verified}
                GROUP BY
                    t."Id",
                    u."FirstName",
                    u."LastName",
                    u."ProfileImageKey",
                    t."Bio",
                    t."YearsOfExperience"
                HAVING AVG(r."Rating") >= {minimumRating}
                ORDER BY u."FirstName", u."LastName"
                LIMIT {pageSize} OFFSET {offset}
                """)
            .ToListAsync(cancellationToken);

        var items = rows
            .Select(ToDto)
            .ToList();

        var totalCount = rows.Count == 0
            ? 0
            : (int)rows[0].TotalCount;

        return new PagedResult<RatedTechnicianDto>(
            Items: items,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount);
    }

    public async Task<IReadOnlyCollection<TechnicianServiceDto>?> GetServicesByTechnicianAsync(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        var isEligible = await _dbContext.TechnicianProfiles
            .AsNoTracking()
            .AnyAsync(profile =>
                profile.Id == technicianProfileId
                && profile.User.AccountStatus == AccountStatus.Active
                && profile.VerificationStatus == VerificationStatus.Verified,
                cancellationToken);

        if (!isEligible)
        {
            return null;
        }

        return await _dbContext.TechnicianServices
            .AsNoTracking()
            .Where(service =>
                service.TechnicianProfileId == technicianProfileId
                && service.ServiceCategory.IsActive)
            .OrderBy(service => service.ServiceCategory.DisplayOrder)
            .ThenBy(service => service.ServiceCategory.Name)
            .Select(service => new TechnicianServiceDto(
                ServiceCategoryId: service.ServiceCategoryId,
                ServiceCategoryName: service.ServiceCategory.Name,
                ServiceCategoryDescription: service.ServiceCategory.Description,
                ServiceCategoryIconKey: service.ServiceCategory.IconKey,
                DisplayOrder: service.ServiceCategory.DisplayOrder))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TechnicianServicePricingResponse>?> GetPricingByTechnicianAsync(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        var isEligible = await _dbContext.TechnicianProfiles
            .AsNoTracking()
            .AnyAsync(profile =>
                profile.Id == technicianProfileId
                && profile.User.AccountStatus == AccountStatus.Active
                && profile.VerificationStatus == VerificationStatus.Verified,
                cancellationToken);

        if (!isEligible)
        {
            return null;
        }

        return await _dbContext.TechnicianServices
            .AsNoTracking()
            .Where(service =>
                service.TechnicianProfileId == technicianProfileId
                && service.ServiceCategory.IsActive)
            .OrderBy(service => service.ServiceCategory.DisplayOrder)
            .ThenBy(service => service.ServiceCategory.Name)
            .Select(service => new TechnicianServicePricingResponse(
                TechnicianServiceId: service.Id,
                TechnicianProfileId: service.TechnicianProfileId,
                ServiceCategoryId: service.ServiceCategoryId,
                ServiceCategoryName: service.ServiceCategory.Name,
                Price: service.Price))
            .ToListAsync(cancellationToken);
    }

    public async Task<TechnicianAvailabilitySettingsResponse?> GetAvailabilityByTechnicianAsync(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.TechnicianProfiles
            .AsNoTracking()
            .Where(profile =>
                profile.Id == technicianProfileId
                && profile.User.AccountStatus == AccountStatus.Active
                && profile.VerificationStatus == VerificationStatus.Verified)
            .Select(profile => new TechnicianAvailabilitySettingsResponse(
                TechnicianProfileId: profile.Id,
                Status: profile.AvailabilitySettings.Status,
                WorkingDays: profile.AvailabilitySettings.WorkingDays
                    .Select(day => new TechnicianWorkingDayResponse(
                        day.Day,
                        day.StartTime,
                        day.EndTime))
                    .ToList(),
                VacationStartDate: profile.AvailabilitySettings.VacationStartDate,
                VacationEndDate: profile.AvailabilitySettings.VacationEndDate))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TechnicianPortfolioItemResponse>?> GetPortfolioByTechnicianAsync(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        var isEligible = await _dbContext.TechnicianProfiles
            .AsNoTracking()
            .AnyAsync(profile =>
                profile.Id == technicianProfileId
                && profile.User.AccountStatus == AccountStatus.Active
                && profile.VerificationStatus == VerificationStatus.Verified,
                cancellationToken);

        if (!isEligible)
        {
            return null;
        }

        return await _dbContext.TechnicianPortfolioItems
            .AsNoTracking()
            .Where(item => item.TechnicianProfileId == technicianProfileId)
            .OrderByDescending(item => item.CreatedAtUtc)
            .Select(item => new TechnicianPortfolioItemResponse(
                PortfolioItemId: item.Id,
                TechnicianProfileId: item.TechnicianProfileId,
                Title: item.Title,
                Description: item.Description,
                Media: item.Media
                    .OrderBy(media => media.DisplayOrder)
                    .Select(media => new TechnicianPortfolioMediaResponse(
                        PortfolioMediaId: media.Id,
                        MediaKey: media.MediaKey,
                        DisplayOrder: media.DisplayOrder))
                    .ToList()))
            .ToListAsync(cancellationToken);
    }

    public async Task<TechnicianVerificationStatusResponse?> GetVerificationStatusByTechnicianAsync(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.TechnicianProfiles
            .AsNoTracking()
            .Where(profile =>
                profile.Id == technicianProfileId
                && profile.User.AccountStatus == AccountStatus.Active
                && profile.VerificationStatus == VerificationStatus.Verified)
            .Select(profile => new TechnicianVerificationStatusResponse(
                TechnicianProfileId: profile.Id,
                Status: profile.VerificationStatus))
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static RatedTechnicianDto ToDto(RatedTechnicianProjection row)
        => new(
            TechnicianProfileId: row.TechnicianProfileId,
            FirstName: row.FirstName,
            LastName: row.LastName,
            ProfileImageKey: row.ProfileImageKey,
            Bio: row.Bio,
            YearsOfExperience: row.YearsOfExperience,
            AverageRating: row.AverageRating);

    private static NearbyTechnicianDto ToDto(TechnicianDiscoveryProjection row)
        => new(
            TechnicianProfileId: row.TechnicianProfileId,
            UserId: row.UserId,
            FirstName: row.FirstName,
            LastName: row.LastName,
            ProfileImageKey: row.ProfileImageKey,
            Bio: row.Bio,
            YearsOfExperience: row.YearsOfExperience,
            DistanceInKm: row.DistanceInKm,
            Latitude: row.Latitude,
            Longitude: row.Longitude);

    private static (double MinLatitude, double MaxLatitude, double MinLongitude, double MaxLongitude)
        BoundingBox(decimal latitude, decimal longitude, double radiusInKm)
    {
        var centerLatitude = (double)latitude;
        var centerLongitude = (double)longitude;

        var deltaLatitude = radiusInKm / KilometersPerDegree;

        var deltaLongitude = radiusInKm
            / (KilometersPerDegree * Math.Cos(DegreesToRadians(centerLatitude)));

        return (
            MinLatitude: centerLatitude - deltaLatitude,
            MaxLatitude: centerLatitude + deltaLatitude,
            MinLongitude: centerLongitude - deltaLongitude,
            MaxLongitude: centerLongitude + deltaLongitude);
    }

    private static double DegreesToRadians(double degrees)
        => degrees * Math.PI / 180.0;

    public sealed class TechnicianDiscoveryProjection
    {
        public Guid TechnicianProfileId { get; set; }

        public Guid UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? ProfileImageKey { get; set; }

        public string? Bio { get; set; }

        public int YearsOfExperience { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public double DistanceInKm { get; set; }

        public long TotalCount { get; set; }
    }

    public sealed class RatedTechnicianProjection
    {
        public Guid TechnicianProfileId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? ProfileImageKey { get; set; }

        public string? Bio { get; set; }

        public int YearsOfExperience { get; set; }

        public double AverageRating { get; set; }

        public long TotalCount { get; set; }
    }
}
