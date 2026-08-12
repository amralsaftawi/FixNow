using FixNow.Api.Mappings.TechnicianProfiles;
using FixNow.Application.Features.TechnicianProfiles.Commands.AddTechnicianExperience;
using FixNow.Application.Features.TechnicianProfiles.Commands.RemoveTechnicianExperience;
using FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianExperience;
using FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianExperiences;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianExperience;

[Route("api/technician-profiles/me/experience")]
public sealed class TechnicianExperienceController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<FixNow.Contracts.Responses.TechnicianExperienceResponse>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyTechnicianExperiences(CancellationToken cancellationToken)
    {
        var query = new GetMyTechnicianExperiencesQuery();

        var result = await sender.Send(query, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponses()),Problem);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.TechnicianExperienceResponse),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddTechnicianExperience([FromBody] AddTechnicianExperienceRequest request,CancellationToken cancellationToken)
    {
        var command = new AddTechnicianExperienceCommand(
            CompanyName: request.CompanyName,
            Position: request.Position,
            Description: request.Description,
            StartDate: request.StartDate,
            EndDate: request.EndDate);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => StatusCode(StatusCodes.Status201Created,response.ToContractResponse()),Problem);
    }

    [HttpPut("{experienceId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.TechnicianExperienceResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTechnicianExperience(
        Guid experienceId,
        [FromBody] UpdateTechnicianExperienceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTechnicianExperienceCommand(
            ExperienceId: experienceId,
            CompanyName: request.CompanyName,
            Position: request.Position,
            Description: request.Description,
            StartDate: request.StartDate,
            EndDate: request.EndDate);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()),Problem);
    }

    [HttpDelete("{experienceId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveTechnicianExperience(
        Guid experienceId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTechnicianExperienceCommand(
            ExperienceId: experienceId);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }
}
