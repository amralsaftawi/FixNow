namespace FixNow.Application.Features.TechnicianRequests.Queries.GetAvailableServiceRequests;

public sealed record AvailableServiceRequestDto(
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
    IReadOnlyCollection<AvailableServiceRequestDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
