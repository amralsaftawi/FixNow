using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByLocation;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByLocation;

public sealed record FilterTechniciansByLocationQuery(
    int CityId,
    int PageNumber = 1,
    int PageSize = 20)
    : IQuery<Result<FilterTechniciansByLocationResponse>>;
