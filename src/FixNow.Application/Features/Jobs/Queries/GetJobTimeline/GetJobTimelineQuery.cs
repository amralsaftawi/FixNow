namespace FixNow.Application.Features.Jobs.Queries.GetJobTimeline;

public sealed record GetJobTimelineQuery(
    Guid JobId,
    int PageNumber,
    int PageSize)
    : IQuery<Result<GetJobTimelineResponse>>;
