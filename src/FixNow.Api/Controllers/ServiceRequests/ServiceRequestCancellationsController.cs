using FixNow.Application.Features.ServiceRequests.Commands.CancelServiceRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.ServiceRequests;

[Route("api/service-requests/{serviceRequestId:guid}/cancellation")]
public sealed class ServiceRequestCancellationsController(ISender sender) : ApiController
{
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelServiceRequest(
        Guid serviceRequestId,
        CancellationToken cancellationToken)
    {
        var command = new CancelServiceRequestCommand(
            ServiceRequestId: serviceRequestId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
