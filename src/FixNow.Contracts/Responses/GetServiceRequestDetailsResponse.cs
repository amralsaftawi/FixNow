namespace FixNow.Contracts.Responses;

public sealed record GetServiceRequestDetailsResponse(
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
    decimal Longitude,
    IReadOnlyCollection<string> ImageKeys);
