using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByRating;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByRating;

public sealed class FilterTechniciansByRatingQueryHandler(
    ITechnicianDiscoveryRepository technicianDiscoveryRepository)
    : IQueryHandler<
        FilterTechniciansByRatingQuery,
        Result<FilterTechniciansByRatingResponse>>
{
    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    public async Task<Result<FilterTechniciansByRatingResponse>> Handle(
        FilterTechniciansByRatingQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _technicianDiscoveryRepository.GetByMinimumRatingAsync(
            minimumRating: query.MinimumRating,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken);

        return new FilterTechniciansByRatingResponse(
            Items: result.Items,
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}
