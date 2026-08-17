using FixNow.Application.Common.Models;
using FixNow.Application.Features.Jobs.Queries.GetCustomerJobEta;

namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface ITechnicianProfileRepository
{
    Task<bool> ExistsByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<TechnicianProfile?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<TechnicianProfile?> GetByUserIdWithServicesAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<TechnicianProfile?> GetByIdWithServicesAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<TechnicianProfile?> GetByUserIdWithPortfolioAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<TechnicianProfile?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<TechnicianLocationDto?> GetLocationAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TechnicianProfile>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        VerificationStatus? verificationStatus = null,
        CancellationToken cancellationToken = default);

    void Update(TechnicianProfile technicianProfile);

    Task AddAsync(
        TechnicianProfile technicianProfile,
        CancellationToken cancellationToken = default);

    Task AddExperienceAsync(
        TechnicianExperience experience,
        CancellationToken cancellationToken = default);

    void RemoveExperience(TechnicianExperience experience);

    Task AddPortfolioItemAsync(
        TechnicianPortfolioItem portfolioItem,
        CancellationToken cancellationToken = default);

    void RemovePortfolioItem(TechnicianPortfolioItem portfolioItem);

    Task AddServiceAsync(
        TechnicianService service,
        CancellationToken cancellationToken = default);

    void RemoveService(TechnicianService service);

    Task<Money?> GetServicePriceByCategoryAsync(
        Guid technicianProfileId,
        Guid serviceCategoryId,
        CancellationToken cancellationToken = default);
}