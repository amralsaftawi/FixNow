using ApplicationTechnicianReviewDto =
    FixNow.Application.Features.Reviews.Queries.GetTechnicianReviews.TechnicianReviewDto;
using ApplicationGetTechnicianReviewsResponse =
    FixNow.Application.Features.Reviews.Queries.GetTechnicianReviews.GetTechnicianReviewsResponse;

namespace FixNow.Api.Mappings.Reviews;

public static class GetTechnicianReviewsMapping
{
    public static FixNow.Contracts.Responses.TechnicianReviewsResponse ToContractResponse(
        this ApplicationGetTechnicianReviewsResponse response)
        => new(
            Items: response.Items
                .Select(ToContractResponse)
                .ToList(),
            PageNumber: response.PageNumber,
            PageSize: response.PageSize,
            TotalCount: response.TotalCount,
            TotalPages: response.TotalPages);

    private static FixNow.Contracts.Responses.TechnicianReviewItemResponse ToContractResponse(
        ApplicationTechnicianReviewDto item)
        => new(
            ReviewId: item.ReviewId,
            JobId: item.JobId,
            Rating: item.Rating,
            Comment: item.Comment,
            CreatedAtUtc: item.CreatedAtUtc);
}
