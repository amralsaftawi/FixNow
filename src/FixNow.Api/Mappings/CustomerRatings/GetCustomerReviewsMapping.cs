using ApplicationCustomerReviewDto =
    FixNow.Application.Features.CustomerRatings.Queries.GetCustomerReviews.CustomerReviewDto;
using ApplicationGetCustomerReviewsResponse =
    FixNow.Application.Features.CustomerRatings.Queries.GetCustomerReviews.GetCustomerReviewsResponse;

namespace FixNow.Api.Mappings.CustomerRatings;

public static class GetCustomerReviewsMapping
{
    public static FixNow.Contracts.Responses.CustomerReviewsResponse ToContractResponse(
        this ApplicationGetCustomerReviewsResponse response)
        => new(
            Items: response.Items
                .Select(ToContractResponse)
                .ToList(),
            PageNumber: response.PageNumber,
            PageSize: response.PageSize,
            TotalCount: response.TotalCount,
            TotalPages: response.TotalPages);

    private static FixNow.Contracts.Responses.CustomerReviewItemResponse ToContractResponse(
        ApplicationCustomerReviewDto item)
        => new(
            CustomerRatingId: item.CustomerRatingId,
            JobId: item.JobId,
            Rating: item.Rating,
            Comment: item.Comment,
            CreatedAtUtc: item.CreatedAtUtc);
}
