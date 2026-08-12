using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianPortfolio;

public sealed class GetMyTechnicianPortfolioQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : IQueryHandler<
        GetMyTechnicianPortfolioQuery,
        Result<List<TechnicianPortfolioItemResponse>>>
{
    public async Task<Result<List<TechnicianPortfolioItemResponse>>> Handle(
        GetMyTechnicianPortfolioQuery query,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await technicianProfileRepository
            .GetByUserIdWithPortfolioAsync(
                currentUser.UserId,
                cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return technicianProfile.PortfolioItems
            .OrderByDescending(item => item.CreatedAtUtc)
            .ToDtos();
    }
}
