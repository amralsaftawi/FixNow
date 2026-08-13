using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianSearch.Queries.SearchTechnicians;

namespace FixNow.Application.Features.TechnicianSearch.Queries.SearchTechnicians;

public sealed class SearchTechniciansQueryHandler(
    ITechnicianSearchRepository technicianSearchRepository)
    : IQueryHandler<SearchTechniciansQuery, Result<SearchTechniciansResponse>>
{
    private readonly ITechnicianSearchRepository _technicianSearchRepository =
        technicianSearchRepository;

    public async Task<Result<SearchTechniciansResponse>> Handle(
        SearchTechniciansQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _technicianSearchRepository.SearchAsync(
            searchTerm: query.SearchTerm.Trim(),
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken);

        return new SearchTechniciansResponse(
            Items: result.Items,
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}
