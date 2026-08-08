using FixNow.Application.Features.CustomerProfiles.Commands.AddCustomerAddress;
using FixNow.Application.Features.CustomerProfiles.Commands.CreateCustomerProfile;
using FixNow.Application.Features.CustomerProfiles.Commands.RemoveCustomerAddress;
using FixNow.Application.Features.CustomerProfiles.Commands.SetDefaultCustomerAddress;
using FixNow.Application.Features.CustomerProfiles.Commands.UpdateCustomerAddress;
using FixNow.Application.Features.CustomerProfiles.Commands.UpdateCurrentCustomerLocation;
using FixNow.Application.Features.CustomerProfiles.Dtos.Responses;
using FixNow.Application.Features.CustomerProfiles.Queries.GetMyCurrentLocation;
using FixNow.Application.Features.CustomerProfiles.Queries.GetMyCustomerProfile;
using FixNow.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixNow.Api.Controllers.CustomerProfiles;

[Route("api/customer-profiles")]
public sealed class CustomerProfilesController(ISender sender) : ApiController
{
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(
        typeof(CustomerProfileResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyCustomerProfile(
        CancellationToken cancellationToken)
    {
        var query = new GetMyCustomerProfileQuery();

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateCustomerProfile(
        CancellationToken cancellationToken)
    {
        var command = new CreateCustomerProfileCommand();

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => StatusCode(StatusCodes.Status201Created),
            Problem);
    }

    [HttpPost("addresses")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddCustomerAddress(
        [FromBody] AddCustomerAddressRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddCustomerAddressCommand(
            Label: request.Label,
            CountryId: request.CountryId,
            CityId: request.CityId,
            AreaId: request.AreaId,
            Street: request.Street,
            BuildingNumber: request.BuildingNumber,
            Floor: request.Floor,
            Apartment: request.Apartment,
            Latitude: request.Latitude,
            Longitude: request.Longitude,
            FullAddress: request.FullAddress);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => StatusCode(StatusCodes.Status201Created),
            Problem);
    }

    [HttpDelete("addresses/{addressId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveCustomerAddress(
        Guid addressId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCustomerAddressCommand(
            addressId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("addresses/{addressId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCustomerAddress(
        Guid addressId,
        [FromBody] UpdateCustomerAddressRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCustomerAddressCommand(
            AddressId: addressId,
            Label: request.Label,
            CountryId: request.CountryId,
            CityId: request.CityId,
            AreaId: request.AreaId,
            Street: request.Street,
            BuildingNumber: request.BuildingNumber,
            Floor: request.Floor,
            Apartment: request.Apartment,
            Latitude: request.Latitude,
            Longitude: request.Longitude,
            FullAddress: request.FullAddress);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("addresses/{addressId:guid}/default")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetDefaultCustomerAddress(
        Guid addressId,
        CancellationToken cancellationToken)
    {
        var command = new SetDefaultCustomerAddressCommand(
            addressId);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpGet("current-location")]
    [Authorize]
    [ProducesResponseType(
        typeof(CurrentLocationResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyCurrentLocation(
        CancellationToken cancellationToken)
    {
        var query = new GetMyCurrentLocationQuery();

        var result = await sender.Send(
            query,
            cancellationToken);

        return result.Match(
            response => Ok(response),
            Problem);
    }

    [HttpPut("current-location")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCurrentCustomerLocation(
        [FromBody] UpdateCurrentCustomerLocationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCurrentCustomerLocationCommand(
            Latitude: request.Latitude,
            Longitude: request.Longitude);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
