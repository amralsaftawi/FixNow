using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByService;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByService;

public sealed record FilterTechniciansByServiceQuery(
    Guid ServiceCategoryId,
    int PageNumber = 1,
    int PageSize = 20)
    : IQuery<Result<FilterTechniciansByServiceResponse>>;
