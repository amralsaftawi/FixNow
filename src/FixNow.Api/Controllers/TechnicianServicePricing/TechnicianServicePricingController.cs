using FixNow.Api.Mappings.TechnicianServicePricing;
using FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianServicePricing;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianServicePricing;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianServicePricing;

[Route("api/technician-services/me")]
public sealed class TechnicianServicePricingController(ISender sender) : ApiController
{
    [HttpGet("pricing")]
    [Authorize]
    [ProducesResponseType(typeof(List<FixNow.Contracts.Responses.TechnicianServicePricingResponse>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyTechnicianServicePricing(CancellationToken cancellationToken)
    {
        var query = new GetMyTechnicianServicePricingQuery();

        var result = await sender.Send(query, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponses()),Problem);
    }

    [HttpPut("{technicianServiceId:guid}/pricing")]
    [Authorize]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.TechnicianServicePricingResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTechnicianServicePricing(
        Guid technicianServiceId,
        [FromBody] UpdateTechnicianServicePricingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTechnicianServicePricingCommand(
            TechnicianServiceId: technicianServiceId,
            Amount: request.Amount,
            Currency: request.Currency);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()),Problem);
    }
}
