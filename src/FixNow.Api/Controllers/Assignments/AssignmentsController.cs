using FixNow.Application.Features.Assignments.Commands.AssignServiceRequest;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.Assignments;

[Route("api/assignments")]
public sealed class AssignmentsController(ISender sender) : ApiController
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignServiceRequest(
        [FromBody] AssignServiceRequestRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignServiceRequestCommand(
            ServiceRequestId: request.ServiceRequestId,
            TechnicianProfileId: request.TechnicianProfileId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
