using FixNow.Api.Mappings.TechnicianRequests;
using FixNow.Application.Features.TechnicianRequests.Commands.AcceptServiceRequest;
using FixNow.Application.Features.TechnicianRequests.Commands.ConvertServiceRequestToJob;
using FixNow.Application.Features.TechnicianRequests.Commands.RejectServiceRequest;
using FixNow.Application.Features.TechnicianRequests.Commands.UpdateTechnicianArrivalStatus;
using FixNow.Application.Features.TechnicianRequests.Queries.GetAvailableServiceRequests;
using FixNow.Application.Features.TechnicianRequests.Queries.GetServiceRequestDetails;
using FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianActiveJobs;
using FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianJobHistory;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianRequests;

[Route("api/technician/requests")]
public sealed class TechnicianRequestsController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.GetAvailableServiceRequestsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAvailableServiceRequests(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAvailableServiceRequestsQuery(
            PageNumber: pageNumber,
            PageSize: pageSize);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }

    [HttpGet("{requestId:guid}")]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.GetServiceRequestDetailsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetServiceRequestDetails(
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetServiceRequestDetailsQuery(requestId);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }

    [HttpGet("active")]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.GetTechnicianActiveJobsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTechnicianActiveJobs(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTechnicianActiveJobsQuery(
            PageNumber: pageNumber,
            PageSize: pageSize);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }

    [HttpGet("history")]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.GetTechnicianJobHistoryResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTechnicianJobHistory(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTechnicianJobHistoryQuery(
            PageNumber: pageNumber,
            PageSize: pageSize);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }

    [HttpPut("{requestId:guid}/accept")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AcceptServiceRequest(
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var command = new AcceptServiceRequestCommand(requestId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{requestId:guid}/reject")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RejectServiceRequest(
        Guid requestId,
        [FromBody] RejectServiceRequestRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new RejectServiceRequestCommand(
            ServiceRequestId: requestId,
            Reason: request.Reason);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{requestId:guid}/arrival-status")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateArrivalStatus(
        Guid requestId,
        [FromBody] UpdateTechnicianArrivalStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateTechnicianArrivalStatusCommand(
            ServiceRequestId: requestId,
            Status: request.Status);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{requestId:guid}/convert-to-job")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ConvertServiceRequestToJob(
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var command = new ConvertServiceRequestToJobCommand(requestId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
