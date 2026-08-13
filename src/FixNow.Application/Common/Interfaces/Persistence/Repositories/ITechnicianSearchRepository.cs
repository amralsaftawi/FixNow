using FixNow.Application.Common.Models;
using FixNow.Application.Features.TechnicianSearch.Queries.SearchTechnicians;

namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface ITechnicianSearchRepository
{
    Task<PagedResult<TechnicianSearchResultDto>> SearchAsync(
        string searchTerm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
