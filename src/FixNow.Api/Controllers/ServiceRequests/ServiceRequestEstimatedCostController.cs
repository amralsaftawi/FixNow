using FixNow.Application.Features.ServiceRequests.Commands.SetEstimatedCost;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.ServiceRequests;

[Route("api/service-requests/{serviceRequestId:guid}/estimated-cost")]
public sealed class ServiceRequestEstimatedCostController(ISender sender) : ApiController
{
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SetEstimatedCost(
        Guid serviceRequestId,
        [FromBody] SetEstimatedCostRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SetEstimatedCostCommand(
            ServiceRequestId: serviceRequestId,
            Amount: request.Amount,
            Currency: request.Currency);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
