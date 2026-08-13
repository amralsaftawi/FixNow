using System.Linq;
using ApplicationFilterTechniciansByLocationResponse =
    FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByLocation.FilterTechniciansByLocationResponse;
using ApplicationLocatedTechnicianDto =
    FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByLocation.LocatedTechnicianDto;
using ContractFilterTechniciansByLocationResponse =
    FixNow.Contracts.Responses.FilterTechniciansByLocationResponse;
using ContractLocatedTechnicianResponse =
    FixNow.Contracts.Responses.LocatedTechnicianResponse;

namespace FixNow.Api.Mappings.TechnicianDiscovery;

public static class FilterTechniciansByLocationMapping
{
    public static ContractFilterTechniciansByLocationResponse ToContractResponse(
        this ApplicationFilterTechniciansByLocationResponse response)
        => new(
            Items: response.Items
                .Select(ToContractResponse)
                .ToList(),
            PageNumber: response.PageNumber,
            PageSize: response.PageSize,
            TotalCount: response.TotalCount,
            TotalPages: response.TotalPages);

    private static ContractLocatedTechnicianResponse ToContractResponse(
        ApplicationLocatedTechnicianDto item)
        => new(
            TechnicianProfileId: item.TechnicianProfileId,
            FirstName: item.FirstName,
            LastName: item.LastName,
            ProfileImageKey: item.ProfileImageKey,
            Bio: item.Bio,
            YearsOfExperience: item.YearsOfExperience);
}
