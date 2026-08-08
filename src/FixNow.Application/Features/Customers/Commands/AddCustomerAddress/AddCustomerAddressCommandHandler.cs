using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.CustomerProfiles.Commands.AddCustomerAddress;

public sealed class AddCustomerAddressCommandHandler(
    ICustomerRepository customerRepository,
    IAreaRepository areaRepository,
    ICurrentUser currentUser)
    : ICommandHandler<AddCustomerAddressCommand, Result<Created>>
{
    public async Task<Result<Created>> Handle(
        AddCustomerAddressCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Find the current user's customer profile.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Validate the geographic references (area -> city -> country).
        var geographicResult = await ValidateGeographicReferenceAsync(
            command,
            cancellationToken);

        if (geographicResult.IsError)
        {
            return geographicResult.Errors;
        }

        // 3. Create the address.
        var addressResult = Address.Create(
            id: Guid.NewGuid(),
            customerProfileId: customerProfile.Id,
            label: command.Label,
            countryId: command.CountryId,
            cityId: command.CityId,
            areaId: command.AreaId,
            street: command.Street,
            buildingNumber: command.BuildingNumber,
            floor: command.Floor,
            apartment: command.Apartment,
            latitude: command.Latitude,
            longitude: command.Longitude,
            fullAddress: command.FullAddress);

        if (addressResult.IsError)
        {
            return addressResult.Errors;
        }

        // 4. Add the address to the profile.
        var addResult = customerProfile.AddAddress(addressResult.Value);

        if (addResult.IsError)
        {
            return addResult.Errors;
        }

        // 5. Persist changes.
        customerRepository.Update(customerProfile);

        return Result.Created;
    }

    private async Task<Result<Success>> ValidateGeographicReferenceAsync(
        AddCustomerAddressCommand command,
        CancellationToken cancellationToken)
    {
        var area = await areaRepository.GetWithCityByIdAsync(
            command.AreaId,
            cancellationToken);

        if (area is null)
        {
            return AddressErrors.AreaNotFound;
        }

        if (area.CityId != command.CityId)
        {
            return AddressErrors.AreaCityMismatch;
        }

        if (area.City is null || area.City.CountryId != command.CountryId)
        {
            return AddressErrors.CityCountryMismatch;
        }

        return Result.Success;
    }
}
