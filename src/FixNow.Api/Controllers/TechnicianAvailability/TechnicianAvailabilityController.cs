using FixNow.Api.Mappings.TechnicianAvailability;
using FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAvailability;
using FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAvailabilitySettings;
using FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianAvailability;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianAvailability;

[Route("api/technician-profiles/me/availability")]
public sealed class TechnicianAvailabilityController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.TechnicianAvailabilitySettingsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyTechnicianAvailability(
        CancellationToken cancellationToken)
    {
        var query = new GetMyTechnicianAvailabilityQuery();

        var result = await sender.Send(query, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()), Problem);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.TechnicianAvailabilitySettingsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTechnicianAvailabilitySettings(
        [FromBody] UpdateTechnicianAvailabilitySettingsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTechnicianAvailabilitySettingsCommand(
            Status: request.Status,
            WorkingDays: request.WorkingDays
                .Select(workingDay => new TechnicianWorkingDayCommand(
                    workingDay.Day,
                    workingDay.StartTime,
                    workingDay.EndTime))
                .ToList(),
            VacationStartDate: request.VacationStartDate,
            VacationEndDate: request.VacationEndDate);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()), Problem);
    }

    [HttpPut("status")]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.TechnicianAvailabilityResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTechnicianAvailability(
        [FromBody] UpdateTechnicianAvailabilityRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTechnicianAvailabilityCommand(
            Availability: request.Availability);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()), Problem);
    }
}
