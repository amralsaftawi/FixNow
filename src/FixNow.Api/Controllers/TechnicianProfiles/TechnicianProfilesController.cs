using FixNow.Api.Mappings.TechnicianProfiles;
using FixNow.Application.Features.TechnicianProfiles.Commands.RegisterTechnician;
using FixNow.Application.Features.TechnicianProfiles.Commands.UpdateMyTechnicianPersonalInformation;
using FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianProfile;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianPersonalInformation;
using FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianProfile;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianProfiles;

[Route("api/technician-profiles")]
public sealed class TechnicianProfilesController(ISender sender) : ApiController
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.RegisterTechnicianResponse),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateTechnicianProfile([FromBody] RegisterTechnicianRequest request,CancellationToken cancellationToken)
    {
        var command = new RegisterTechnicianCommand(
            YearsOfExperience: request.YearsOfExperience,
            Bio: request.Bio,
            NationalIdImageKey: request.NationalIdImageKey,
            ServiceCategoryIds: request.ServiceCategoryIds);

        var result = await sender.Send( command, cancellationToken);

        return result.Match( response => StatusCode(StatusCodes.Status201Created,response.ToContractResponse()),
            Problem);
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(TechnicianProfileResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyTechnicianProfile(CancellationToken cancellationToken)
    {
        var query = new GetMyTechnicianProfileQuery();

        var result = await sender.Send(query,cancellationToken);

        return result.Match(response => Ok(response),Problem);
    }

    [HttpPut("me")]
    [Authorize]
    [ProducesResponseType(typeof(TechnicianProfileResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTechnicianProfile([FromBody] UpdateTechnicianProfileRequest request,CancellationToken cancellationToken)
    {
        var command = new UpdateTechnicianProfileCommand(
            YearsOfExperience: request.YearsOfExperience,
            Bio: request.Bio,
            NationalIdImageKey: request.NationalIdImageKey);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => Ok(response),Problem);
    }

    [HttpGet("me/personal-info")]
    [Authorize]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.TechnicianPersonalInformationResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyTechnicianPersonalInformation(CancellationToken cancellationToken)
    {
        var query = new GetMyTechnicianPersonalInformationQuery();

        var result = await sender.Send(query,cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()),Problem);
    }

    [HttpPut("me/personal-info")]
    [Authorize]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.TechnicianPersonalInformationResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateMyTechnicianPersonalInformation([FromBody] UpdateTechnicianPersonalInformationRequest request,CancellationToken cancellationToken)
    {
        var command = new UpdateMyTechnicianPersonalInformationCommand(
            FirstName: request.FirstName,
            LastName: request.LastName,
            Email: request.Email,
            PhoneNumber: request.PhoneNumber,
            CountryCode: request.CountryCode,
            PreferredLanguage: request.PreferredLanguage);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()),Problem);
    }
}
