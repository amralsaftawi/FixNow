namespace FixNow.Contracts.Responses;

public sealed record CreateServiceRequestResponse(
    Guid Id,
    Guid CustomerProfileId,
    Guid AddressId,
    Guid ServiceCategoryId,
    string Description,
    ServicePriority Priority,
    ServiceRequestStatus Status,
    DateTimeOffset RequestedAt,
    DateTimeOffset? ScheduledAt);
