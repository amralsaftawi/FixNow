using FixNow.Application.Features.TechnicianProfiles.Commands.SubmitForVerification;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianVerification;

[Route("api/technician-profiles/me/verification")]
public sealed class TechnicianVerificationController(ISender sender) : ApiController
{
    [HttpPost("submit")]
    [Authorize]
    [ProducesResponseType(typeof(TechnicianProfileResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SubmitForVerification(CancellationToken cancellationToken)
    {
        var command = new SubmitForVerificationCommand();

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => Ok(response), Problem);
    }
}
