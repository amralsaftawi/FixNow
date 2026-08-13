using FixNow.Application.Features.ServiceRequests.Commands.SelectProblemType;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.ServiceRequests;

[Route("api/service-requests/{serviceRequestId:guid}/problem-type")]
public sealed class ServiceRequestProblemTypesController(ISender sender) : ApiController
{
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SelectProblemType(
        Guid serviceRequestId,
        [FromBody] SelectProblemTypeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SelectProblemTypeCommand(
            ServiceRequestId: serviceRequestId,
            ProblemTypeId: request.ProblemTypeId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
