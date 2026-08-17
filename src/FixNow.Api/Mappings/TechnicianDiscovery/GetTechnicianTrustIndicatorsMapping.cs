using ApplicationGetTechnicianTrustIndicatorsResponse =
    FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianTrustIndicators.GetTechnicianTrustIndicatorsResponse;

namespace FixNow.Api.Mappings.TechnicianDiscovery;

public static class GetTechnicianTrustIndicatorsMapping
{
    public static FixNow.Contracts.Responses.TechnicianTrustIndicatorsResponse ToContractResponse(
        this ApplicationGetTechnicianTrustIndicatorsResponse response)
        => new(
            TechnicianProfileId: response.TechnicianProfileId,
            IsVerified: response.IsVerified,
            IsProfileComplete: response.IsProfileComplete,
            IsActive: response.IsActive,
            YearsOfExperience: response.YearsOfExperience,
            AverageRating: response.AverageRating,
            TotalRatings: response.TotalRatings);
}
