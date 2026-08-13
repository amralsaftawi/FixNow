namespace FixNow.Application.Features.TechnicianRequests.Queries.GetServiceRequestDetails;

public sealed record GetServiceRequestDetailsDto(
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
    List<string> ImageKeys);

public sealed record GetServiceRequestDetailsResponse(
    GetServiceRequestDetailsDto Details);
