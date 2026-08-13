using FixNow.Application.Features.ServiceRequests.Commands.SetPreferredServiceTime;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.ServiceRequests;

[Route("api/service-requests/{serviceRequestId:guid}/preferred-service-time")]
public sealed class ServiceRequestPreferredServiceTimesController(ISender sender) : ApiController
{
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SetPreferredServiceTime(
        Guid serviceRequestId,
        [FromBody] SetPreferredServiceTimeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SetPreferredServiceTimeCommand(
            ServiceRequestId: serviceRequestId,
            PreferredServiceTime: request.PreferredServiceTime!.Value);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
