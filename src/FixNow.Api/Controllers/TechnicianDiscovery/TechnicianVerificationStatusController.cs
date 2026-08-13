using FixNow.Api.Mappings.TechnicianDiscovery;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianVerificationStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianDiscovery;

[Route("api/technician-discovery/technicians/{technicianProfileId}/verification-status")]
public sealed class TechnicianVerificationStatusController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.TechnicianVerificationStatusResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTechnicianVerificationStatus(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTechnicianVerificationStatusQuery(
            TechnicianProfileId: technicianProfileId);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }
}
