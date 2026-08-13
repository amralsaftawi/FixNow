using System.Linq;
using ApplicationTechnicianServicesResponse =
    FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianServices.TechnicianServicesResponse;
using ApplicationTechnicianServiceDto =
    FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianServices.TechnicianServiceDto;
using ContractTechnicianServicesResponse =
    FixNow.Contracts.Responses.TechnicianServicesResponse;
using ContractTechnicianDiscoveryServiceResponse =
    FixNow.Contracts.Responses.TechnicianDiscoveryServiceResponse;

namespace FixNow.Api.Mappings.TechnicianDiscovery;

public static class GetTechnicianServicesMapping
{
    public static ContractTechnicianServicesResponse ToContractResponse(
        this ApplicationTechnicianServicesResponse response)
        => new(
            Items: response.Items
                .Select(ToContractResponse)
                .ToList());

    private static ContractTechnicianDiscoveryServiceResponse ToContractResponse(
        ApplicationTechnicianServiceDto item)
        => new(
            ServiceCategoryId: item.ServiceCategoryId,
            ServiceCategoryName: item.ServiceCategoryName,
            ServiceCategoryDescription: item.ServiceCategoryDescription,
            ServiceCategoryIconKey: item.ServiceCategoryIconKey,
            DisplayOrder: item.DisplayOrder);
}
