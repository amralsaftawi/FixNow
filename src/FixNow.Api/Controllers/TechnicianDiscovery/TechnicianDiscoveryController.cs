using FixNow.Api.Mappings.TechnicianDiscovery;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianDiscovery;

[Route("api/technician-discovery/nearby")]
public sealed class TechnicianDiscoveryController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.FindNearbyTechniciansResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> FindNearbyTechnicians(
        [FromQuery] decimal latitude,
        [FromQuery] decimal longitude,
        [FromQuery] double radiusInKm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new FindNearbyTechniciansQuery(
            Latitude: latitude,
            Longitude: longitude,
            RadiusInKm: radiusInKm,
            PageNumber: pageNumber,
            PageSize: pageSize);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }
}
