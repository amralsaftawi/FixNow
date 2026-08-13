using System.Linq;
using FixNow.Api.Mappings.TechnicianServicePricing;
using ApplicationTechnicianPricingResponse =
    FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPricing.TechnicianPricingResponse;
using ContractTechnicianPricingResponse =
    FixNow.Contracts.Responses.TechnicianPricingResponse;

namespace FixNow.Api.Mappings.TechnicianDiscovery;

public static class GetTechnicianPricingMapping
{
    public static ContractTechnicianPricingResponse ToContractResponse(
        this ApplicationTechnicianPricingResponse response)
        => new(
            Items: response.Items
                .Select(item => item.ToContractResponse())
                .ToList());
}
