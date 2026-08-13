using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPortfolio;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPortfolio;

public sealed class GetTechnicianPortfolioQueryHandler(
    ITechnicianDiscoveryRepository technicianDiscoveryRepository)
    : IQueryHandler<GetTechnicianPortfolioQuery, Result<TechnicianPortfolioResponse>>
{
    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    public async Task<Result<TechnicianPortfolioResponse>> Handle(
        GetTechnicianPortfolioQuery query,
        CancellationToken cancellationToken)
    {
        var portfolio = await _technicianDiscoveryRepository.GetPortfolioByTechnicianAsync(
            technicianProfileId: query.TechnicianProfileId,
            cancellationToken: cancellationToken);

        if (portfolio is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return new TechnicianPortfolioResponse(
            Items: portfolio);
    }
}
