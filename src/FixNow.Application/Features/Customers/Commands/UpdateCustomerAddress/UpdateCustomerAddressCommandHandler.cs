using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.CustomerProfiles.Commands.UpdateCustomerAddress;

public sealed class UpdateCustomerAddressCommandHandler(
    ICustomerRepository customerRepository,
    IAreaRepository areaRepository,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateCustomerAddressCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(
        UpdateCustomerAddressCommand command,
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

        // 3. Find the address within the current user's profile.
        var address = customerProfile.Addresses
            .FirstOrDefault(item => item.Id == command.AddressId);

        if (address is null)
        {
            return CustomerProfileErrors.AddressNotFound;
        }

        // 4. Update the address.
        var updateResult = address.Update(
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

        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        // 5. Persist changes.
        customerRepository.Update(customerProfile);

        return Result.Updated;
    }

    private async Task<Result<Success>> ValidateGeographicReferenceAsync(
        UpdateCustomerAddressCommand command,
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
