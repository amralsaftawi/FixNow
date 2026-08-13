using System.Linq;
using ApplicationFilterTechniciansByRatingResponse =
    FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByRating.FilterTechniciansByRatingResponse;
using ApplicationRatedTechnicianDto =
    FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByRating.RatedTechnicianDto;
using ContractFilterTechniciansByRatingResponse =
    FixNow.Contracts.Responses.FilterTechniciansByRatingResponse;
using ContractRatedTechnicianResponse =
    FixNow.Contracts.Responses.RatedTechnicianResponse;

namespace FixNow.Api.Mappings.TechnicianDiscovery;

public static class FilterTechniciansByRatingMapping
{
    public static ContractFilterTechniciansByRatingResponse ToContractResponse(
        this ApplicationFilterTechniciansByRatingResponse response)
        => new(
            Items: response.Items
                .Select(ToContractResponse)
                .ToList(),
            PageNumber: response.PageNumber,
            PageSize: response.PageSize,
            TotalCount: response.TotalCount,
            TotalPages: response.TotalPages);

    private static ContractRatedTechnicianResponse ToContractResponse(
        ApplicationRatedTechnicianDto item)
        => new(
            TechnicianProfileId: item.TechnicianProfileId,
            FirstName: item.FirstName,
            LastName: item.LastName,
            ProfileImageKey: item.ProfileImageKey,
            Bio: item.Bio,
            YearsOfExperience: item.YearsOfExperience,
            AverageRating: item.AverageRating);
}
