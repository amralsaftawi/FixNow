using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceAvailability;

public sealed record GetServiceAvailabilityQuery(
    Guid ServiceCategoryId)
    : IQuery<Result<ServiceAvailabilityResponse>>;
