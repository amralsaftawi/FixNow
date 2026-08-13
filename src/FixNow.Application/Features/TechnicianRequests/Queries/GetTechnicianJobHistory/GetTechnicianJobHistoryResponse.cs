namespace FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianJobHistory;

public sealed record HistoricalServiceRequestDto(
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
    IReadOnlyCollection<HistoricalServiceRequestDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
