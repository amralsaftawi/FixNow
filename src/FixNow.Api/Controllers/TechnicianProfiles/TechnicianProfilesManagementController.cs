using FixNow.Application.Features.TechnicianProfiles.Commands.RejectTechnicianVerification;
using FixNow.Application.Features.TechnicianProfiles.Commands.VerifyTechnician;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Queries.GetTechnicianProfiles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianProfiles;

[Route("api/admin/technician-profiles")]
public sealed class TechnicianProfilesManagementController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(
        typeof(TechnicianProfilesResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetTechnicianProfiles(
        [FromQuery] VerificationStatus? verificationStatus,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTechnicianProfilesQuery(
            VerificationStatus: verificationStatus,
            PageNumber: pageNumber,
            PageSize: pageSize);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPost("{technicianProfileId:guid}/verify")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(
        typeof(TechnicianProfileResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> VerifyTechnician(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        var command = new VerifyTechnicianCommand(
            technicianProfileId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPost("{technicianProfileId:guid}/reject")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(
        typeof(TechnicianProfileResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RejectTechnicianVerification(
        Guid technicianProfileId,
        CancellationToken cancellationToken = default)
    {
        var command = new RejectTechnicianVerificationCommand(
            technicianProfileId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }
}
