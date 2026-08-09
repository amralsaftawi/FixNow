using FixNow.Api.Mappings.Identity;
using FixNow.Application.Features.Identity.Commands.ForgotPassword;
using FixNow.Application.Features.Identity.Commands.ResetPassword;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.Authentication;

[Route("api/auth/password")]
public sealed class PasswordController(ISender sender) : ApiController
{
    [HttpPost("forgot")]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.ForgotPasswordResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ForgotPasswordCommand(
            request.Identifier);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }

    [HttpPost("reset")]
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
            response => Ok(response.ToContractResponse()),
            Problem);
    }
}
