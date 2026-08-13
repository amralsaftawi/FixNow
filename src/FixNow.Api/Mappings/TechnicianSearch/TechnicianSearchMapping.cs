using System.Linq;
using ApplicationSearchTechniciansResponse =
    FixNow.Application.Features.TechnicianSearch.Queries.SearchTechnicians.SearchTechniciansResponse;
using ApplicationTechnicianSearchResultDto =
    FixNow.Application.Features.TechnicianSearch.Queries.SearchTechnicians.TechnicianSearchResultDto;
using ContractSearchTechniciansResponse =
    FixNow.Contracts.Responses.SearchTechniciansResponse;
using ContractTechnicianSearchResultResponse =
    FixNow.Contracts.Responses.TechnicianSearchResultResponse;

namespace FixNow.Api.Mappings.TechnicianSearch;

public static class TechnicianSearchMapping
{
    public static ContractSearchTechniciansResponse ToContractResponse(
        this ApplicationSearchTechniciansResponse response)
        => new(
            Items: response.Items
                .Select(ToContractResponse)
                .ToList(),
            PageNumber: response.PageNumber,
            PageSize: response.PageSize,
            TotalCount: response.TotalCount,
            TotalPages: response.TotalPages);

    private static ContractTechnicianSearchResultResponse ToContractResponse(
        ApplicationTechnicianSearchResultDto item)
        => new(
            TechnicianProfileId: item.TechnicianProfileId,
            FirstName: item.FirstName,
            LastName: item.LastName,
            ProfileImageKey: item.ProfileImageKey,
            Bio: item.Bio,
            YearsOfExperience: item.YearsOfExperience);
}
