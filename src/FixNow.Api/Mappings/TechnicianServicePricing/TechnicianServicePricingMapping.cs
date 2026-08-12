using System.Collections.Generic;
using System.Linq;
using ApplicationTechnicianServicePricingResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianServicePricingResponse;
using ContractTechnicianServicePricingResponse =
    FixNow.Contracts.Responses.TechnicianServicePricingResponse;

namespace FixNow.Api.Mappings.TechnicianServicePricing;

public static class TechnicianServicePricingMapping
{
    public static ContractTechnicianServicePricingResponse ToContractResponse(
        this ApplicationTechnicianServicePricingResponse response)
        => new(
            TechnicianServiceId: response.TechnicianServiceId,
            TechnicianProfileId: response.TechnicianProfileId,
            ServiceCategoryId: response.ServiceCategoryId,
            ServiceCategoryName: response.ServiceCategoryName,
            Price: response.Price);

    public static List<ContractTechnicianServicePricingResponse> ToContractResponses(
        this IEnumerable<ApplicationTechnicianServicePricingResponse> responses)
        => responses.Select(ToContractResponse).ToList();
}
