namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceAvailability;

public sealed record ServiceAvailabilityResponse(
    Guid ServiceCategoryId,
    bool IsAvailable);
