using System.Collections.Generic;
using System.Linq;
using ApplicationTechnicianServiceResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianServiceResponse;
using ContractTechnicianServiceResponse =
    FixNow.Contracts.Responses.TechnicianServiceResponse;

namespace FixNow.Api.Mappings.TechnicianProfiles;

public static class TechnicianServiceMapping
{
    public static ContractTechnicianServiceResponse ToContractResponse(
        this ApplicationTechnicianServiceResponse response)
        => new(
            TechnicianServiceId: response.TechnicianServiceId,
            TechnicianProfileId: response.TechnicianProfileId,
            ServiceCategoryId: response.ServiceCategoryId,
            ServiceCategoryName: response.ServiceCategoryName,
            ServiceCategoryDescription: response.ServiceCategoryDescription,
            ServiceCategoryIconKey: response.ServiceCategoryIconKey,
            ServiceCategoryDisplayOrder: response.ServiceCategoryDisplayOrder,
            ServiceCategoryPrice: response.ServiceCategoryPrice,
            ServiceCategoryIsActive: response.ServiceCategoryIsActive);

    public static List<ContractTechnicianServiceResponse> ToContractResponses(
        this IEnumerable<ApplicationTechnicianServiceResponse> responses)
        => responses.Select(ToContractResponse).ToList();
}
