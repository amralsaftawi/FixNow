using FixNow.Api.Mappings.TechnicianAvailability;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianAvailability;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianDiscovery;

[Route("api/technician-discovery/technicians/{technicianProfileId}/availability")]
public sealed class TechnicianAvailabilityController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.TechnicianAvailabilitySettingsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTechnicianAvailability(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTechnicianAvailabilityQuery(
            TechnicianProfileId: technicianProfileId);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }
}
