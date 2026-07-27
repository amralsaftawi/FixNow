using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians;

public sealed record FindNearbyTechniciansQuery(
    Guid ServiceCategoryId,
    decimal Latitude,
    decimal Longitude,
    double RadiusInKm,
    int PageNumber = 1,
    int PageSize = 20)
    : IQuery<Result<FindNearbyTechniciansResponse>>;