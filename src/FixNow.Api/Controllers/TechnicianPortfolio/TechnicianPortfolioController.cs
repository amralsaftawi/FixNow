using FixNow.Api.Mappings.TechnicianPortfolio;
using FixNow.Application.Features.TechnicianProfiles.Commands.CreateTechnicianPortfolioItem;
using FixNow.Application.Features.TechnicianProfiles.Commands.RemoveTechnicianPortfolioItem;
using FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianPortfolioItem;
using FixNow.Application.Features.TechnicianProfiles.Commands.UploadTechnicianPortfolioMedia;
using FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianPortfolio;
using FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianPortfolioItem;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianPortfolio;

[Route("api/technician-profiles/me/portfolio")]
public sealed class TechnicianPortfolioController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<FixNow.Contracts.Responses.TechnicianPortfolioItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyTechnicianPortfolio(CancellationToken cancellationToken)
    {
        var query = new GetMyTechnicianPortfolioQuery();

        var result = await sender.Send(query, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponses()), Problem);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.TechnicianPortfolioItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateTechnicianPortfolioItem(
        [FromBody] CreateTechnicianPortfolioItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTechnicianPortfolioItemCommand(
            Title: request.Title,
            Description: request.Description,
            MediaKeys: request.MediaKeys);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response.ToContractResponse()),
            Problem);
    }

    [HttpGet("{portfolioItemId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.TechnicianPortfolioItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyTechnicianPortfolioItem(
        Guid portfolioItemId,
        CancellationToken cancellationToken)
    {
        var query = new GetMyTechnicianPortfolioItemQuery(PortfolioItemId: portfolioItemId);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()), Problem);
    }

    [HttpPut("{portfolioItemId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.TechnicianPortfolioItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTechnicianPortfolioItem(
        Guid portfolioItemId,
        [FromBody] UpdateTechnicianPortfolioItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTechnicianPortfolioItemCommand(
            PortfolioItemId: portfolioItemId,
            Title: request.Title,
            Description: request.Description,
            MediaKeys: request.MediaKeys);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()), Problem);
    }

    [HttpDelete("{portfolioItemId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveTechnicianPortfolioItem(
        Guid portfolioItemId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTechnicianPortfolioItemCommand(
            PortfolioItemId: portfolioItemId);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPost("media")]
    [Authorize]
    [ProducesResponseType(typeof(UploadTechnicianPortfolioMediaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadTechnicianPortfolioMedia(
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        await using var content = file.OpenReadStream();

        var command = new UploadTechnicianPortfolioMediaCommand(
            Content: content,
            FileName: file.FileName,
            ContentType: file.ContentType,
            ContentLength: file.Length);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response),
            Problem);
    }
}
