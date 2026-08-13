namespace FixNow.Contracts.Responses;

public sealed record HistoricalServiceRequestResponse(
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

public sealed record GetTechnicianJobHistoryResponse(
    IReadOnlyCollection<HistoricalServiceRequestResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
