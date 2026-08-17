using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Reviews.Queries.GetTechnicianReviews;

public sealed record GetTechnicianReviewsQuery(
    Guid TechnicianProfileId,
    int PageNumber = 1,
    int PageSize = 20)
    : IQuery<Result<GetTechnicianReviewsResponse>>;
