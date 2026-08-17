using FixNow.Api.Mappings.Reviews;
using FixNow.Application.Features.Reviews.Commands.CreateReview;
using FixNow.Application.Features.Reviews.Commands.HideReview;
using FixNow.Application.Features.Reviews.Commands.ReportReview;
using FixNow.Application.Features.Reviews.Commands.RestoreReview;
using FixNow.Application.Features.Reviews.Queries.GetTechnicianReviews;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.Reviews;

[ApiController]
[Route("api/reviews")]
public sealed class ReviewsController(ISender sender) : ApiController
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateReviewResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateReview(
        [FromBody] CreateReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateReviewCommand(
            JobId: request.JobId,
            Comment: request.Comment);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response),
            Problem);
    }

    [HttpGet("technicians/{technicianProfileId:guid}")]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.TechnicianReviewsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTechnicianReviews(
        Guid technicianProfileId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTechnicianReviewsQuery(
            TechnicianProfileId: technicianProfileId,
            PageNumber: pageNumber,
            PageSize: pageSize);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }

    [HttpPost("{reviewId:guid}/reports")]
    [Authorize]
    [ProducesResponseType(typeof(ReportReviewResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ReportReview(
        Guid reviewId,
        [FromBody] ReportReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new ReportReviewCommand(
            ReviewId: reviewId,
            Reason: request.Reason,
            Description: request.Description);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response),
            Problem);
    }

    [HttpPatch("{reviewId:guid}/hide")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(HideReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> HideReview(
        Guid reviewId,
        CancellationToken cancellationToken = default)
    {
        var command = new HideReviewCommand(
            ReviewId: reviewId);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPatch("{reviewId:guid}/restore")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RestoreReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RestoreReview(
        Guid reviewId,
        CancellationToken cancellationToken = default)
    {
        var command = new RestoreReviewCommand(
            ReviewId: reviewId);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }
}
