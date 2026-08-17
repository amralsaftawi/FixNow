using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianRating;

public sealed class GetTechnicianRatingQueryHandler(
    ITechnicianDiscoveryRepository technicianDiscoveryRepository)
    : IQueryHandler<GetTechnicianRatingQuery, Result<GetTechnicianRatingResponse>>
{
    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    public async Task<Result<GetTechnicianRatingResponse>> Handle(
        GetTechnicianRatingQuery query,
        CancellationToken cancellationToken)
    {
        var summary =
            await _technicianDiscoveryRepository.GetRatingSummaryByTechnicianAsync(
                technicianProfileId: query.TechnicianProfileId,
                cancellationToken: cancellationToken);

        if (summary is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return new GetTechnicianRatingResponse(
            TechnicianProfileId: query.TechnicianProfileId,
            AverageRating: summary.AverageRating,
            RatingCount: summary.RatingCount);
    }
}
