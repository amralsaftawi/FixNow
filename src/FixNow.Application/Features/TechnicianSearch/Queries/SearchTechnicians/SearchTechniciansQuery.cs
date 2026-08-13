using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianSearch.Queries.SearchTechnicians;

namespace FixNow.Application.Features.TechnicianSearch.Queries.SearchTechnicians;

public sealed record SearchTechniciansQuery(
    string SearchTerm,
    int PageNumber = 1,
    int PageSize = 20)
    : IQuery<Result<SearchTechniciansResponse>>;
