using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByRating;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.FilterTechniciansByRating;

public sealed record FilterTechniciansByRatingQuery(
    double MinimumRating,
    int PageNumber = 1,
    int PageSize = 20)
    : IQuery<Result<FilterTechniciansByRatingResponse>>;
