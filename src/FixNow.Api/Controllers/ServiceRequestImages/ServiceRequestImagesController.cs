using FixNow.Application.Features.ServiceRequests.Commands.UploadServiceRequestImage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.ServiceRequestImages;

[Route("api/service-requests/{serviceRequestId:guid}/images")]
public sealed class ServiceRequestImagesController(ISender sender) : ApiController
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(UploadServiceRequestImageResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadServiceRequestImage(
        Guid serviceRequestId,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        await using var content = file.OpenReadStream();

        var command = new UploadServiceRequestImageCommand(
            ServiceRequestId: serviceRequestId,
            Content: content,
            FileName: file.FileName,
            ContentType: file.ContentType,
            ContentLength: file.Length);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response),
            Problem);
    }
}
