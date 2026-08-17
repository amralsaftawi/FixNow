using FixNow.Application.Features.Payments.Commands.CreateCashPayment;
using FixNow.Application.Features.Payments.Commands.InitiateOnlinePayment;
using FixNow.Application.Features.Payments.Commands.ProcessPayment;
using FixNow.Application.Features.Payments.Commands.RefundPayment;
using FixNow.Application.Features.Payments.Queries.GetPaymentStatus;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.Payments;

[ApiController]
[Route("api/payments")]
public sealed class PaymentsController(ISender sender) : ApiController
{
    [HttpPost("cash")]
    [Authorize]
    [ProducesResponseType(typeof(CashPaymentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateCashPayment(
        [FromBody] CreateCashPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateCashPaymentCommand(
            JobId: request.JobId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response),
            Problem);
    }

    [HttpPost("online")]
    [Authorize]
    [ProducesResponseType(typeof(OnlinePaymentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InitiateOnlinePayment(
        [FromBody] InitiateOnlinePaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new InitiateOnlinePaymentCommand(
            JobId: request.JobId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response),
            Problem);
    }

    [HttpPost("{paymentId:guid}/process")]
    [Authorize]
    [ProducesResponseType(typeof(ProcessPaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ProcessPayment(
        [FromRoute] Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        var command = new ProcessPaymentCommand(
            PaymentId: paymentId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPost("{paymentId:guid}/refund")]
    [Authorize]
    [ProducesResponseType(typeof(RefundPaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefundPayment(
        [FromRoute] Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        var command = new RefundPaymentCommand(
            PaymentId: paymentId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpGet("{paymentId:guid}/status")]
    [Authorize]
    [ProducesResponseType(typeof(GetPaymentStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentStatus(
        [FromRoute] Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPaymentStatusQuery(
            PaymentId: paymentId);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }
}
