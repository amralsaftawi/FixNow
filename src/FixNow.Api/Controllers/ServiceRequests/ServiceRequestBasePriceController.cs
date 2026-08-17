using FixNow.Application.Features.ServiceRequests.Queries.GetBaseServicePrice;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.ServiceRequests;

[Route("api/service-requests/{serviceRequestId:guid}/base-price")]
public sealed class ServiceRequestBasePriceController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(
        typeof(GetBaseServicePriceResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBaseServicePrice(
        Guid serviceRequestId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetBaseServicePriceQuery(serviceRequestId);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }
}
