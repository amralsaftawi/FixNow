using FixNow.Api.Mappings.TechnicianDiscovery;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPricing;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianDiscovery;

[Route("api/technician-discovery/technicians/{technicianProfileId}/pricing")]
public sealed class TechnicianPricingController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.TechnicianPricingResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTechnicianPricing(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTechnicianPricingQuery(
            TechnicianProfileId: technicianProfileId);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }
}
