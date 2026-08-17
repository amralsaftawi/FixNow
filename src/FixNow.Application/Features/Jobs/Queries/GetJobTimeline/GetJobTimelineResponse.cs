namespace FixNow.Application.Features.Jobs.Queries.GetJobTimeline;

public sealed record JobTimelineEntryDto(
    Guid Id,
    JobStatus Status,
    string Description,
    DateTimeOffset OccurredOn);

public sealed record GetJobTimelineResponse(
    IReadOnlyCollection<JobTimelineEntryDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
