using FixNow.Application.Features.Jobs.Commands.AddAdditionalServiceCharge;
using FixNow.Application.Features.Jobs.Commands.CancelJob;
using FixNow.Application.Features.Jobs.Commands.ConfirmServiceCompletion;
using FixNow.Application.Features.Jobs.Commands.MarkJobArrived;
using FixNow.Application.Features.Jobs.Commands.MarkJobCompleted;
using FixNow.Application.Features.Jobs.Commands.MarkJobEnRoute;
using FixNow.Application.Features.Jobs.Commands.MarkJobPaused;
using FixNow.Application.Features.Jobs.Commands.MarkJobStarted;
using FixNow.Application.Features.Jobs.Commands.RateTechnician;
using FixNow.Application.Features.Jobs.Commands.UpdateJobStatus;
using FixNow.Application.Features.Jobs.Commands.UpdateTechnicianLocation;
using FixNow.Application.Features.Jobs.Queries.GetCustomerJobEta;
using FixNow.Application.Features.Jobs.Queries.GetCustomerJobTracking;
using FixNow.Application.Features.Jobs.Queries.GetFinalJobPrice;
using FixNow.Application.Features.Jobs.Queries.GetJobTimeline;
using FixNow.Application.Features.CustomerRatings.Commands.RateCustomer;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.Jobs;

[Route("api/jobs")]
public sealed class JobsController(ISender sender) : ApiController
{
    [HttpPut("{jobId:guid}/status")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateJobStatus(
        Guid jobId,
        [FromBody] UpdateJobStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateJobStatusCommand(
            JobId: jobId,
            Status: request.Status);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{jobId:guid}/en-route")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> MarkJobEnRoute(
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        var command = new MarkJobEnRouteCommand(jobId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{jobId:guid}/arrived")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> MarkJobArrived(
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        var command = new MarkJobArrivedCommand(jobId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{jobId:guid}/start")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> MarkJobStarted(
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        var command = new MarkJobStartedCommand(jobId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{jobId:guid}/pause")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> MarkJobPaused(
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        var command = new MarkJobPausedCommand(jobId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{jobId:guid}/complete")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> MarkJobCompleted(
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        var command = new MarkJobCompletedCommand(jobId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{jobId:guid}/cancel")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelJob(
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        var command = new CancelJobCommand(jobId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{jobId:guid}/location")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTechnicianLocation(
        Guid jobId,
        [FromBody] UpdateTechnicianLocationRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateTechnicianLocationCommand(
            JobId: jobId,
            Latitude: request.Latitude,
            Longitude: request.Longitude);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpGet("{jobId:guid}/tracking")]
    [Authorize]
    [ProducesResponseType(typeof(GetCustomerJobTrackingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerJobTracking(
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCustomerJobTrackingQuery(jobId);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpGet("{jobId:guid}/eta")]
    [Authorize]
    [ProducesResponseType(typeof(GetCustomerJobEtaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerJobEta(
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCustomerJobEtaQuery(jobId);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpGet("{jobId:guid}/timeline")]
    [Authorize]
    [ProducesResponseType(typeof(GetJobTimelineResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobTimeline(
        Guid jobId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetJobTimelineQuery(
            JobId: jobId,
            PageNumber: pageNumber,
            PageSize: pageSize);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpGet("{jobId:guid}/price")]
    [Authorize]
    [ProducesResponseType(typeof(GetFinalJobPriceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFinalJobPrice(
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetFinalJobPriceQuery(jobId);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPost("{jobId:guid}/completion/confirm")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ConfirmServiceCompletion(
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        var command = new ConfirmServiceCompletionCommand(jobId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPost("{jobId:guid}/additional-charges")]
    [Authorize]
    [ProducesResponseType(typeof(AdditionalServiceChargeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddAdditionalServiceCharge(
        Guid jobId,
        [FromBody] AddAdditionalServiceChargeRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new AddAdditionalServiceChargeCommand(
            JobId: jobId,
            Description: request.Description,
            Amount: request.Amount,
            Currency: request.Currency);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response),
            Problem);
    }

    [HttpPost("{jobId:guid}/rating")]
    [Authorize]
    [ProducesResponseType(typeof(RateTechnicianResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RateTechnician(
        Guid jobId,
        [FromBody] RateTechnicianRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new RateTechnicianCommand(
            JobId: jobId,
            Rating: request.Rating,
            Comment: request.Comment);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response),
            Problem);
    }

    [HttpPost("{jobId:guid}/customer-rating")]
    [Authorize]
    [ProducesResponseType(typeof(RateCustomerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RateCustomer(
        Guid jobId,
        [FromBody] RateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new RateCustomerCommand(
            JobId: jobId,
            Rating: request.Rating);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response),
            Problem);
    }
}
