using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianPortfolioItem;

public sealed class GetMyTechnicianPortfolioItemQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : IQueryHandler<
        GetMyTechnicianPortfolioItemQuery,
        Result<TechnicianPortfolioItemResponse>>
{
    public async Task<Result<TechnicianPortfolioItemResponse>> Handle(
        GetMyTechnicianPortfolioItemQuery query,
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

        var portfolioItem = technicianProfile.PortfolioItems
            .FirstOrDefault(item => item.Id == query.PortfolioItemId);

        if (portfolioItem is null)
        {
            return TechnicianProfileErrors.PortfolioItemNotFound;
        }

        return portfolioItem.ToTechnicianPortfolioItemResponse();
    }
}
