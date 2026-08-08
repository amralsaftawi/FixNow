using FixNow.Application.Features.GeographicData.Commands.CreateArea;
using FixNow.Application.Features.GeographicData.Commands.CreateCity;
using FixNow.Application.Features.GeographicData.Commands.CreateCountry;
using FixNow.Application.Features.GeographicData.Dtos.Responses;
using FixNow.Application.Features.GeographicData.Queries.GetAreasByCity;
using FixNow.Application.Features.GeographicData.Queries.GetCitiesByCountry;
using FixNow.Application.Features.GeographicData.Queries.GetCountries;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.GeographicData;

[Route("api/geographic-data")]
public sealed class GeographicDataController(ISender sender) : ApiController
{
    [HttpGet("countries")]
    [ProducesResponseType(
        typeof(List<CountryResponse>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCountries(
        CancellationToken cancellationToken)
    {
        var query = new GetCountriesQuery();

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPost("countries")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateCountry(
        [FromBody] CreateCountryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCountryCommand(
            Name: request.Name);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => StatusCode(StatusCodes.Status201Created),
            Problem);
    }

    [HttpGet("countries/{countryId:int}/cities")]
    [ProducesResponseType(
        typeof(List<CityResponse>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCitiesByCountry(
        int countryId,
        CancellationToken cancellationToken)
    {
        var query = new GetCitiesByCountryQuery(
            countryId);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPost("countries/{countryId:int}/cities")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCity(
        int countryId,
        [FromBody] CreateCityRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCityCommand(
            CountryId: countryId,
            Name: request.Name);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => StatusCode(StatusCodes.Status201Created),
            Problem);
    }

    [HttpGet("cities/{cityId:int}/areas")]
    [ProducesResponseType(
        typeof(List<AreaResponse>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAreasByCity(
        int cityId,
        CancellationToken cancellationToken)
    {
        var query = new GetAreasByCityQuery(
            cityId);

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPost("cities/{cityId:int}/areas")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateArea(
        int cityId,
        [FromBody] CreateAreaRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateAreaCommand(
            CityId: cityId,
            Name: request.Name);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => StatusCode(StatusCodes.Status201Created),
            Problem);
    }
}
