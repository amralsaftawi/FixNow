
using FixNow.Application.Features.Identity.Commands.Login;
using FixNow.Application.Features.Identity.Commands.Logout;
using FixNow.Application.Features.Identity.Commands.RefreshToken;
using FixNow.Application.Features.Identity.Commands.Register;
using FixNow.Application.Features.Identity.Commands.SendOtp;
using FixNow.Api.Mappings.Identity;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using FixNow.Application.Features.Identity.Commands.VerifyOtp;
using FixNow.Application.Features.Identity.Commands.ResendOtp;
using Microsoft.AspNetCore.Authorization;
using FixNow.Application.Features.Identity.Queries.GetCurrentUser;
using FixNow.Application.Features.Identity.Commands.ForgotPassword;
using FixNow.Application.Features.Identity.Commands.ResetPassword;
using FixNow.Application.Features.Identity.Commands.ChangePassword;
using FixNow.Application.Features.Identity.Commands.UpdateCurrentUser;
using FixNow.Application.Features.Identity.Commands.DeactivateCurrentUser;

namespace FixNow.Api.Controllers.Authentication;

[Route("api/auth")]
public sealed class AuthenticationController(ISender sender) : ApiController
{
    [HttpPost("register")]
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

       return result.Match(response => StatusCode(StatusCodes.Status201Created, response),Problem);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login( [FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(Identifier: request.Identifier,Password: request.Password);

        var result = await sender.Send(command, cancellationToken);

       return result.Match(response => Ok(response),Problem);
    }
   



[HttpPost("refresh-token")]
[ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request,CancellationToken cancellationToken)
{
    var command = new RefreshTokenCommand(RefreshToken: request.RefreshToken);

    var result = await sender.Send(command,cancellationToken);

    return result.Match(response => Ok(response),Problem);
}

[HttpPost("logout")]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken cancellationToken)
{
    var command = new LogoutCommand(request.RefreshToken);

    var result = await sender.Send(command, cancellationToken);

    return result.Match(_ => NoContent(), Problem);
}

[HttpPost("send-otp")]
[ProducesResponseType(typeof(FixNow.Contracts.Responses.SendOtpResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request, CancellationToken cancellationToken)
{
    var command = new SendOtpCommand(request.Identifier);

    var result = await sender.Send(command, cancellationToken);

    return result.Match(response => Ok(response.ToContractResponse()), Problem);
}

[HttpPost("verify-otp")]
[ProducesResponseType(typeof(FixNow.Contracts.Responses.VerifyOtpResponse), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request, CancellationToken cancellationToken)
{
    var command = new VerifyOtpCommand(request.Identifier, request.Otp, request.Purpose);

    var result = await sender.Send(command, cancellationToken);

    return result.Match(response => Ok(response.ToContractResponse()), Problem);
}

[HttpPost("resend-otp")]
[ProducesResponseType(typeof(FixNow.Contracts.Responses.ResendOtpResponse),StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<IActionResult> ResendOtp([FromBody] ResendOtpRequest request,CancellationToken cancellationToken)
{
    var command = new ResendOtpCommand(
        request.Identifier,
        request.Purpose);

    var result = await sender.Send(
        command,
        cancellationToken);

    return result.Match(
        response => Ok(response),Problem);
}


[HttpGet("me")]
[Authorize]
[ProducesResponseType(typeof(FixNow.Contracts.Responses.GetCurrentUserResponse),StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<IActionResult> GetCurrentUser( CancellationToken cancellationToken)
{
    var query = new GetCurrentUserQuery();

    var result = await sender.Send(query,cancellationToken);

    return result.Match(
        response => Ok(response),
        Problem);
}

[HttpPut("me")]
[Authorize]
[ProducesResponseType(typeof(FixNow.Contracts.Responses.UpdateCurrentUserResponse),StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateCurrentUserRequest request, CancellationToken cancellationToken)
{
    var command = new UpdateCurrentUserCommand(
        FirstName: request.FirstName,
        LastName: request.LastName,
        Email: request.Email,
        PhoneNumber: request.PhoneNumber,
        CountryCode: request.CountryCode,
        PreferredLanguage: request.PreferredLanguage);

    var result = await sender.Send(
        command,
        cancellationToken);

    return result.Match(
        response => Ok(response),
        Problem);
}

[HttpPost("deactivate")]
[Authorize]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
public async Task<IActionResult> DeactivateCurrentUser(CancellationToken cancellationToken)
{
    var command = new DeactivateCurrentUserCommand();

    var result = await sender.Send(command, cancellationToken);

    return result.Match(_ => NoContent(), Problem);
}


[HttpPost("forgot-password")]
[ProducesResponseType(
    typeof(FixNow.Contracts.Responses.ForgotPasswordResponse),StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request,
    CancellationToken cancellationToken)
{
    var command = new ForgotPasswordCommand(
        request.Identifier);

    var result = await sender.Send(
        command,
        cancellationToken);

    return result.Match(
        response => Ok(response),
        Problem);
}



[HttpPost("reset-password")]
[ProducesResponseType(
    typeof(FixNow.Contracts.Responses.ResetPasswordResponse),
    StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public async Task<IActionResult> ResetPassword(
    [FromBody] ResetPasswordRequest request,
    CancellationToken cancellationToken)
{
    var command = new ResetPasswordCommand(
        Identifier: request.Identifier,
        Otp: request.Otp,
        NewPassword: request.NewPassword,
        ConfirmPassword: request.ConfirmPassword);

    var result = await sender.Send(
        command,
        cancellationToken);

    return result.Match(
        response => Ok(response),
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

    var result = await sender.Send(
        command,
        cancellationToken);

    return result.Match(
        response => Ok(response),
        Problem);
}
}