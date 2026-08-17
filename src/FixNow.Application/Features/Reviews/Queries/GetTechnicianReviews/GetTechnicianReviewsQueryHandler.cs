using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Reviews.Queries.GetTechnicianReviews;

public sealed class GetTechnicianReviewsQueryHandler(
    ITechnicianDiscoveryRepository technicianDiscoveryRepository,
    IReviewRepository reviewRepository)
    : IQueryHandler<GetTechnicianReviewsQuery, Result<GetTechnicianReviewsResponse>>
{
    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    private readonly IReviewRepository _reviewRepository = reviewRepository;

    public async Task<Result<GetTechnicianReviewsResponse>> Handle(
        GetTechnicianReviewsQuery query,
        CancellationToken cancellationToken)
    {
        var technicianExists =
            await _technicianDiscoveryRepository.ExistsByTechnicianIdAsync(
                technicianProfileId: query.TechnicianProfileId,
                cancellationToken: cancellationToken);

        if (!technicianExists)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var pagedResult =
            await _reviewRepository.GetByTechnicianIdPagedAsync(
                technicianProfileId: query.TechnicianProfileId,
                pageNumber: query.PageNumber,
                pageSize: query.PageSize,
                cancellationToken: cancellationToken);

        return new GetTechnicianReviewsResponse(
            Items: pagedResult.Items,
            PageNumber: pagedResult.PageNumber,
            PageSize: pagedResult.PageSize,
            TotalCount: pagedResult.TotalCount,
            TotalPages: pagedResult.TotalPages);
    }
}
