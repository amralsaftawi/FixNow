namespace FixNow.Contracts.Responses;

public sealed record AvailableServiceRequestResponse(
    Guid ServiceRequestId,
    Guid ServiceCategoryId,
    string ServiceCategoryName,
    Guid? ProblemTypeId,
    string? ProblemTypeName,
    string Description,
    ServicePriority Priority,
    DateTimeOffset RequestedAt,
    DateTimeOffset? ScheduledAt,
    Money? EstimatedCost,
    string FullAddress,
    decimal Latitude,
    decimal Longitude);

public sealed record GetAvailableServiceRequestsResponse(
    IReadOnlyCollection<AvailableServiceRequestResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
