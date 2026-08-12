using FixNow.Api.Mappings.TechnicianAccountStatus;
using FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAccountStatus;
using FixNow.Application.Features.TechnicianProfiles.Queries.GetTechnicianAccountStatus;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianAccountStatus;

[Route("api/admin/technician-account-status")]
public sealed class TechnicianAccountStatusController(ISender sender) : ApiController
{
    [HttpGet("{technicianProfileId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.TechnicianAccountStatusResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTechnicianAccountStatus(
        Guid technicianProfileId,
        CancellationToken cancellationToken)
    {
        var query = new GetTechnicianAccountStatusQuery(technicianProfileId);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()), Problem);
    }

    [HttpPut("{technicianProfileId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.TechnicianAccountStatusResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTechnicianAccountStatus(
        Guid technicianProfileId,
        [FromBody] UpdateTechnicianAccountStatusRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTechnicianAccountStatusCommand(
            technicianProfileId,
            request.Status);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()), Problem);
    }
}
