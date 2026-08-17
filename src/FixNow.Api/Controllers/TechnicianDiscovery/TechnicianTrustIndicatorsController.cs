using FixNow.Api.Mappings.TechnicianDiscovery;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianTrustIndicators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianDiscovery;

[Route("api/technician-discovery")]
public sealed class TechnicianTrustIndicatorsController(ISender sender) : ApiController
{
    [HttpGet("technicians/{technicianProfileId:guid}/trust-indicators")]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.TechnicianTrustIndicatorsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTechnicianTrustIndicators(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTechnicianTrustIndicatorsQuery(
            TechnicianProfileId: technicianProfileId);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }
}
