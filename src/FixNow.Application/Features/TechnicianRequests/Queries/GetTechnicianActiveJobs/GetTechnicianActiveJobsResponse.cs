namespace FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianActiveJobs;

public sealed record ActiveServiceRequestDto(
    Guid ServiceRequestId,
    Guid ServiceCategoryId,
    string ServiceCategoryName,
    Guid? ProblemTypeId,
    string? ProblemTypeName,
    string Description,
    ServicePriority Priority,
    ServiceRequestStatus Status,
    DateTimeOffset RequestedAt,
    DateTimeOffset? ScheduledAt,
    Money? EstimatedCost,
    string FullAddress,
    decimal Latitude,
    decimal Longitude);

public sealed record GetTechnicianActiveJobsResponse(
    IReadOnlyCollection<ActiveServiceRequestDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
