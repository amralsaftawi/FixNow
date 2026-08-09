using FixNow.Api.Mappings.Identity;
using FixNow.Application.Features.Identity.Commands.ChangePassword;
using FixNow.Application.Features.Identity.Commands.DeactivateCurrentUser;
using FixNow.Application.Features.Identity.Commands.UpdateCurrentUser;
using FixNow.Application.Features.Identity.Queries.GetCurrentUser;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.Users;

[Route("api/users/me")]
public sealed class UsersController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.GetCurrentUserResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUser(
        CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserQuery();

        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.UpdateCurrentUserResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateCurrentUser(
        [FromBody] UpdateCurrentUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCurrentUserCommand(
            FirstName: request.FirstName,
            LastName: request.LastName,
            Email: request.Email,
            PhoneNumber: request.PhoneNumber,
            CountryCode: request.CountryCode,
            PreferredLanguage: request.PreferredLanguage);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }

    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.ChangePasswordResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand(
            CurrentPassword: request.CurrentPassword,
            NewPassword: request.NewPassword,
            ConfirmPassword: request.ConfirmPassword);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeactivateCurrentUser(
        CancellationToken cancellationToken)
    {
        var command = new DeactivateCurrentUserCommand();

        var result = await sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }
}
