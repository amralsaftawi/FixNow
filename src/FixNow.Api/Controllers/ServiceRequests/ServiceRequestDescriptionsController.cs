using FixNow.Application.Features.ServiceRequests.Commands.AddProblemDescription;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.ServiceRequests;

[Route("api/service-requests/{serviceRequestId:guid}/description")]
public sealed class ServiceRequestDescriptionsController(ISender sender) : ApiController
{
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddProblemDescription(
        Guid serviceRequestId,
        [FromBody] AddProblemDescriptionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddProblemDescriptionCommand(
            ServiceRequestId: serviceRequestId,
            Description: request.Description);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
