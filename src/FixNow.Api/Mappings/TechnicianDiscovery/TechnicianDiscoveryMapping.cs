using System.Linq;
using ApplicationFindNearbyTechniciansResponse =
    FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians.FindNearbyTechniciansResponse;
using ApplicationNearbyTechnicianDto =
    FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians.NearbyTechnicianDto;
using ContractFindNearbyTechniciansResponse =
    FixNow.Contracts.Responses.FindNearbyTechniciansResponse;
using ContractNearbyTechnicianResponse =
    FixNow.Contracts.Responses.NearbyTechnicianResponse;

namespace FixNow.Api.Mappings.TechnicianDiscovery;

public static class TechnicianDiscoveryMapping
{
    public static ContractFindNearbyTechniciansResponse ToContractResponse(
        this ApplicationFindNearbyTechniciansResponse response)
        => new(
            Items: response.Items
                .Select(ToContractResponse)
                .ToList(),
            PageNumber: response.PageNumber,
            PageSize: response.PageSize,
            TotalCount: response.TotalCount,
            TotalPages: response.TotalPages);

    private static ContractNearbyTechnicianResponse ToContractResponse(
        ApplicationNearbyTechnicianDto item)
        => new(
            TechnicianProfileId: item.TechnicianProfileId,
            UserId: item.UserId,
            FirstName: item.FirstName,
            LastName: item.LastName,
            ProfileImageKey: item.ProfileImageKey,
            Bio: item.Bio,
            YearsOfExperience: item.YearsOfExperience,
            DistanceInKm: item.DistanceInKm,
            Latitude: item.Latitude,
            Longitude: item.Longitude);
}
