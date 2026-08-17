using FixNow.Application.Features.ServiceCategories.Commands.ActivateServiceCategory;
using FixNow.Application.Features.ServiceCategories.Commands.ConfigureServiceCategoryInspectionFee;
using FixNow.Application.Features.ServiceCategories.Commands.ConfigureServiceCategoryPricing;
using FixNow.Application.Features.ServiceCategories.Commands.CreateServiceCategory;
using FixNow.Application.Features.ServiceCategories.Commands.DeactivateServiceCategory;
using FixNow.Application.Features.ServiceCategories.Commands.RemoveServiceCategoryIcon;
using FixNow.Application.Features.ServiceCategories.Commands.UpdateServiceCategory;
using FixNow.Application.Features.ServiceCategories.Commands.UploadServiceCategoryIcon;
using FixNow.Application.Features.ServiceCategories.Queries.GetServiceCategories;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.ServiceCategories;

[ApiController]
[Route("api/admin/service-categories")]
public sealed class ServiceCategoriesManagementController(ISender sender) : ApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(
        typeof(GetServiceCategoriesResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetServiceCategories(
        [FromQuery] string? search,
        [FromQuery] bool? isActive,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetServiceCategoriesQuery(
            Search: search,
            IsActive: isActive,
            PageNumber: pageNumber,
            PageSize: pageSize);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(
        typeof(CreateServiceCategoryResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateServiceCategory(
        [FromBody] CreateServiceCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateServiceCategoryCommand(
            Name: request.Name,
            Description: request.Description,
            IconKey: request.IconKey,
            DisplayOrder: request.DisplayOrder);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => StatusCode(
                StatusCodes.Status201Created,
                response),
            Problem);
    }

    [HttpPut("{serviceCategoryId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateServiceCategory(
        Guid serviceCategoryId,
        [FromBody] UpdateServiceCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateServiceCategoryCommand(
            ServiceCategoryId: serviceCategoryId,
            Name: request.Name,
            Description: request.Description,
            IconKey: request.IconKey,
            DisplayOrder: request.DisplayOrder);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{serviceCategoryId:guid}/pricing")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ConfigureServiceCategoryPricing(
        Guid serviceCategoryId,
        [FromBody] ConfigureServiceCategoryPricingRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new ConfigureServiceCategoryPricingCommand(
            ServiceCategoryId: serviceCategoryId,
            Amount: request.Amount,
            Currency: request.Currency);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{serviceCategoryId:guid}/inspection-fee")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ConfigureServiceCategoryInspectionFee(
        Guid serviceCategoryId,
        [FromBody] ConfigureServiceCategoryInspectionFeeRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new ConfigureServiceCategoryInspectionFeeCommand(
            ServiceCategoryId: serviceCategoryId,
            Amount: request.Amount,
            Currency: request.Currency);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{serviceCategoryId:guid}/icon")]
    [Authorize]
    [ProducesResponseType(
        typeof(UploadServiceCategoryIconResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UploadServiceCategoryIcon(
        Guid serviceCategoryId,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken = default)
    {
        await using var content = file.OpenReadStream();

        var command = new UploadServiceCategoryIconCommand(
            ServiceCategoryId: serviceCategoryId,
            Content: content,
            FileName: file.FileName,
            ContentType: file.ContentType,
            ContentLength: file.Length);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpDelete("{serviceCategoryId:guid}/icon")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RemoveServiceCategoryIcon(
        Guid serviceCategoryId,
        CancellationToken cancellationToken = default)
    {
        var command = new RemoveServiceCategoryIconCommand(
            serviceCategoryId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPost("{serviceCategoryId:guid}/activate")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ActivateServiceCategory(
        Guid serviceCategoryId,
        CancellationToken cancellationToken = default)
    {
        var command = new ActivateServiceCategoryCommand(
            serviceCategoryId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPost("{serviceCategoryId:guid}/deactivate")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeactivateServiceCategory(
        Guid serviceCategoryId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeactivateServiceCategoryCommand(
            serviceCategoryId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
