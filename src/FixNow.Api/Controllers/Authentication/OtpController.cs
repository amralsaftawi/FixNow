using FixNow.Api.Mappings.Identity;
using FixNow.Application.Features.Identity.Commands.ResendOtp;
using FixNow.Application.Features.Identity.Commands.SendOtp;
using FixNow.Application.Features.Identity.Commands.VerifyOtp;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.Authentication;

[Route("api/auth/otp")]
public sealed class OtpController(ISender sender) : ApiController
{
    [HttpPost("send")]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.SendOtpResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request,CancellationToken cancellationToken)
    {
        var command = new SendOtpCommand(request.Identifier);

        var result = await sender.Send(command,cancellationToken);

        return result.Match(response => Ok(response.ToContractResponse()),Problem);
    }

    [HttpPost("verify")]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.VerifyOtpResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyOtp(
        [FromBody] VerifyOtpRequest request,
        CancellationToken cancellationToken)
    {
        var command = new VerifyOtpCommand(
            request.Identifier,
            request.Otp,
            request.Purpose);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }

    [HttpPost("resend")]
    [ProducesResponseType(
        typeof(FixNow.Contracts.Responses.ResendOtpResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResendOtp(
        [FromBody] ResendOtpRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ResendOtpCommand(
            request.Identifier,
            request.Purpose);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => Ok(response.ToContractResponse()),
            Problem);
    }
}
