using FixNow.Api.Mappings.Identity;
using FixNow.Application.Features.Identity.Commands.Login;
using FixNow.Application.Features.Identity.Commands.Logout;
using FixNow.Application.Features.Identity.Commands.RefreshToken;
using FixNow.Application.Features.Identity.Commands.Register;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.Authentication;

[Route("api/auth")]
public sealed class AuthenticationController(ISender sender) : ApiController
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.RegisterResponse),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request,CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(
            FirstName: request.FirstName,
            LastName: request.LastName,
            Email: request.Email,
            PhoneNumber: request.PhoneNumber,
            Password: request.Password,
            ConfirmPassword: request.ConfirmPassword,
            CountryCode: request.CountryCode,
            PreferredLanguage: request.PreferredLanguage);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => StatusCode(StatusCodes.Status201Created,response.ToContractResponse()), Problem);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.LoginResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request,CancellationToken cancellationToken)
    {
        var command = new LoginCommand(Identifier: request.Identifier,Password: request.Password);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()),Problem);
    }



    [HttpPost("refresh")]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.RefreshTokenResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request,CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand( RefreshToken: request.RefreshToken);

        var result = await sender.Send(command, cancellationToken);

        return result.Match( response => Ok(response.ToContractResponse()), Problem);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request,CancellationToken cancellationToken)
    {
        var command = new LogoutCommand(request.RefreshToken);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }
}
