using FixNow.Api.Mappings.ServiceRequests;
using FixNow.Application.Features.Assignments.Commands.ReassignServiceRequest;
using FixNow.Application.Features.Assignments.Commands.UnassignServiceRequest;
using FixNow.Application.Features.ServiceRequests.Commands.CreateServiceRequest;
using FixNow.Contracts.Requests;
using FixNow.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.ServiceRequests;

[Route("api/service-requests")]
public sealed class ServiceRequestsController(ISender sender) : ApiController
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.CreateServiceRequestResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateServiceRequest(
        [FromBody] CreateServiceRequestRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateServiceRequestCommand(
            AddressId: request.AddressId,
            ServiceCategoryId: request.ServiceCategoryId,
            Description: request.Description,
            Priority: request.Priority,
            ScheduledAt: request.ScheduledAt);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response.ToContractResponse()),
            Problem);
    }

    [HttpPut("{requestId:guid}/assignment")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ReassignServiceRequest(
        Guid requestId,
        [FromBody] ReassignServiceRequestRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReassignServiceRequestCommand(
            ServiceRequestId: requestId,
            TechnicianProfileId: request.TechnicianProfileId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpDelete("{requestId:guid}/assignment")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UnassignServiceRequest(
        Guid requestId,
        CancellationToken cancellationToken)
    {
        var command = new UnassignServiceRequestCommand(requestId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
