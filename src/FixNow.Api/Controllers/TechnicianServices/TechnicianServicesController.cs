using FixNow.Api.Mappings.TechnicianProfiles;
using FixNow.Application.Features.TechnicianProfiles.Commands.AddTechnicianService;
using FixNow.Application.Features.TechnicianProfiles.Commands.RemoveTechnicianService;
using FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianServices;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.TechnicianServices;

[Route("api/technician-profiles/me/services")]
public sealed class TechnicianServicesController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<FixNow.Contracts.Responses.TechnicianServiceResponse>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyTechnicianServices(CancellationToken cancellationToken)
    {
        var query = new GetMyTechnicianServicesQuery();

        var result = await sender.Send(query,cancellationToken);

        return result.Match(response => Ok(response.ToContractResponses()),Problem);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(FixNow.Contracts.Responses.TechnicianServiceResponse),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddTechnicianService([FromBody] AddTechnicianServiceRequest request,CancellationToken cancellationToken)
    {
        var command = new AddTechnicianServiceCommand(
            ServiceCategoryId: request.ServiceCategoryId);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(response => StatusCode(StatusCodes.Status201Created,response.ToContractResponse()),Problem);
    }

    [HttpDelete("{serviceCategoryId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveTechnicianService(
        Guid serviceCategoryId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTechnicianServiceCommand(
            ServiceCategoryId: serviceCategoryId);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }
}
