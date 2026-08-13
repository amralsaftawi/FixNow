using System.Linq;
using FixNow.Api.Mappings.TechnicianPortfolio;
using ApplicationTechnicianPortfolioResponse =
    FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPortfolio.TechnicianPortfolioResponse;
using ContractTechnicianPortfolioResponse =
    FixNow.Contracts.Responses.TechnicianPortfolioResponse;

namespace FixNow.Api.Mappings.TechnicianDiscovery;

public static class GetTechnicianPortfolioMapping
{
    public static ContractTechnicianPortfolioResponse ToContractResponse(
        this ApplicationTechnicianPortfolioResponse response)
        => new(
            Items: response.Items
                .Select(item => item.ToContractResponse())
                .ToList());
}
