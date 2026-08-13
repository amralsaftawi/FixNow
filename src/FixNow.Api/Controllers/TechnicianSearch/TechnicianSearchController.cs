using FixNow.Api.Mappings.TechnicianSearch;
using FixNow.Application.Features.TechnicianSearch.Queries.SearchTechnicians;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianSearch;

[Route("api/technician-search")]
public sealed class TechnicianSearchController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.SearchTechniciansResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SearchTechnicians(
        [FromQuery] string searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchTechniciansQuery(
            SearchTerm: searchTerm,
            PageNumber: pageNumber,
            PageSize: pageSize);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }
}
