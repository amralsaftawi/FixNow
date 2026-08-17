using ApplicationGetTechnicianRatingResponse =
    FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianRating.GetTechnicianRatingResponse;

namespace FixNow.Api.Mappings.TechnicianDiscovery;

public static class GetTechnicianRatingMapping
{
    public static FixNow.Contracts.Responses.TechnicianRatingResponse ToContractResponse(
        this ApplicationGetTechnicianRatingResponse response)
        => new(
            TechnicianProfileId: response.TechnicianProfileId,
            AverageRating: response.AverageRating,
            RatingCount: response.RatingCount);
}
