using FixNow.Application.Features.ServiceRequests.Commands.SelectServiceCategory;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.ServiceRequests;

[Route("api/service-requests/{serviceRequestId:guid}/service-category")]
public sealed class ServiceRequestCategoriesController(ISender sender) : ApiController
{
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SelectServiceCategory(
        Guid serviceRequestId,
        [FromBody] SelectServiceCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SelectServiceCategoryCommand(
            ServiceRequestId: serviceRequestId,
            ServiceCategoryId: request.ServiceCategoryId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
