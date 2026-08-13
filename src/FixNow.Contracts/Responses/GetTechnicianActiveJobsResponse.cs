namespace FixNow.Contracts.Responses;

public sealed record ActiveServiceRequestResponse(
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
    IReadOnlyCollection<ActiveServiceRequestResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
