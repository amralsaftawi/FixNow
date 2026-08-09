using FixNow.Application.Features.ServiceCategories.Queries.FilterServiceCategories;
using FixNow.Application.Features.ServiceCategories.Queries.GetActiveServiceCategories;
using FixNow.Application.Features.ServiceCategories.Queries.GetServiceAvailability;
using FixNow.Application.Features.ServiceCategories.Queries.GetServiceCategoryById;
using FixNow.Application.Features.ServiceCategories.Queries.SearchServiceCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.ServiceCategories;

[ApiController]
[Route("api/service-categories")]
public sealed class ServiceCategoriesController(ISender sender) : ApiController
{
    [HttpGet("{serviceCategoryId:guid}/availability")]
    [ProducesResponseType(
        typeof(ServiceAvailabilityResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetServiceAvailability(
        Guid serviceCategoryId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetServiceAvailabilityQuery(
            serviceCategoryId);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpGet("{serviceCategoryId:guid}")]
    [ProducesResponseType(
        typeof(GetServiceCategoryByIdResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetServiceCategoryById(
        Guid serviceCategoryId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetServiceCategoryByIdQuery(
            serviceCategoryId);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpGet("active")]
    [ProducesResponseType(
        typeof(GetActiveServiceCategoriesResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetActiveServiceCategories(
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetActiveServiceCategoriesQuery(
            Search: search,
            PageNumber: pageNumber,
            PageSize: pageSize);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpGet("search")]
    [ProducesResponseType(
        typeof(SearchServiceCategoriesResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchServiceCategories(
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchServiceCategoriesQuery(
            Search: search,
            PageNumber: pageNumber,
            PageSize: pageSize);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpGet("filter")]
    [ProducesResponseType(
        typeof(FilterServiceCategoriesResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FilterServiceCategories(
        [FromQuery] string? search,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] ServiceCategorySortBy sortBy = ServiceCategorySortBy.Default,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new FilterServiceCategoriesQuery(
            Search: search,
            MinPrice: minPrice,
            MaxPrice: maxPrice,
            SortBy: sortBy,
            PageNumber: pageNumber,
            PageSize: pageSize);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }
}
