using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianServicePricing;

public sealed class GetMyTechnicianServicePricingQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetMyTechnicianServicePricingQuery, Result<List<TechnicianServicePricingResponse>>>
{
    public async Task<Result<List<TechnicianServicePricingResponse>>> Handle(
        GetMyTechnicianServicePricingQuery query,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await technicianProfileRepository
            .GetByUserIdWithServicesAsync(
                currentUser.UserId,
                cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return technicianProfile.Services
            .OrderBy(service => service.ServiceCategory.DisplayOrder)
            .ThenBy(service => service.ServiceCategory.Name)
            .ToPricingDtos();
    }
}
