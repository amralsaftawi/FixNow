using System.Linq;
using ApplicationFilterTechniciansByServiceResponse =
    FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByService.FilterTechniciansByServiceResponse;
using ApplicationServiceTechnicianDto =
    FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByService.ServiceTechnicianDto;
using ContractFilterTechniciansByServiceResponse =
    FixNow.Contracts.Responses.FilterTechniciansByServiceResponse;
using ContractServiceTechnicianResponse =
    FixNow.Contracts.Responses.ServiceTechnicianResponse;

namespace FixNow.Api.Mappings.TechnicianDiscovery;

public static class FilterTechniciansByServiceMapping
{
    public static ContractFilterTechniciansByServiceResponse ToContractResponse(
        this ApplicationFilterTechniciansByServiceResponse response)
        => new(
            Items: response.Items
                .Select(ToContractResponse)
                .ToList(),
            PageNumber: response.PageNumber,
            PageSize: response.PageSize,
            TotalCount: response.TotalCount,
            TotalPages: response.TotalPages);

    private static ContractServiceTechnicianResponse ToContractResponse(
        ApplicationServiceTechnicianDto item)
        => new(
            TechnicianProfileId: item.TechnicianProfileId,
            FirstName: item.FirstName,
            LastName: item.LastName,
            ProfileImageKey: item.ProfileImageKey,
            Bio: item.Bio,
            YearsOfExperience: item.YearsOfExperience);
}
