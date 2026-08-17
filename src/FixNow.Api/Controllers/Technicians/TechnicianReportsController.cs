using FixNow.Application.Features.TechnicianReports.Commands.ReportTechnician;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.Technicians;

[ApiController]
[Route("api/technicians")]
public sealed class TechnicianReportsController(ISender sender) : ApiController
{
    [HttpPost("{technicianProfileId:guid}/reports")]
    [Authorize]
    [ProducesResponseType(typeof(ReportTechnicianResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ReportTechnician(
        Guid technicianProfileId,
        [FromBody] ReportTechnicianRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new ReportTechnicianCommand(
            TechnicianProfileId: technicianProfileId,
            Reason: request.Reason,
            Description: request.Description);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response),
            Problem);
    }
}
