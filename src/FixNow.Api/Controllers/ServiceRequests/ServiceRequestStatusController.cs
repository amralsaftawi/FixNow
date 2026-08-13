using FixNow.Application.Features.ServiceRequests.Commands.CompleteServiceRequest;
using FixNow.Application.Features.ServiceRequests.Commands.MarkSearching;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.ServiceRequests;

[Route("api/service-requests/{serviceRequestId:guid}/status")]
public sealed class ServiceRequestStatusController(ISender sender) : ApiController
{
    [HttpPut("searching")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> MarkSearching(
        Guid serviceRequestId,
        CancellationToken cancellationToken)
    {
        var command = new MarkSearchingCommand(
            ServiceRequestId: serviceRequestId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("completed")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CompleteServiceRequest(
        Guid serviceRequestId,
        CancellationToken cancellationToken)
    {
        var command = new CompleteServiceRequestCommand(
            ServiceRequestId: serviceRequestId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
