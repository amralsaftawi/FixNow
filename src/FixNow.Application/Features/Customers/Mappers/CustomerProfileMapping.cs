using System.Linq;
using FixNow.Application.Features.CustomerProfiles.Dtos.Responses;

namespace FixNow.Application.Features.CustomerProfiles.Mappers;

public static class CustomerProfileMapping
{
    public static CustomerProfileResponse ToCustomerProfileResponse(
        this CustomerProfile entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CustomerProfileResponse(
            CustomerProfileId: entity.Id,
            UserId: entity.UserId,
            RegisteredAt: entity.RegisteredAt,
            Addresses: entity.Addresses
                .Select(ToAddressResponse)
                .ToList(),
            PaymentMethods: entity.PaymentMethods
                .Select(ToPaymentMethodResponse)
                .ToList());
    }

    public static AddressResponse ToAddressResponse(
        this Address address)
    {
        ArgumentNullException.ThrowIfNull(address);

        return new AddressResponse(
            AddressId: address.Id,
            Label: address.Label,
            CountryId: address.CountryId,
            CityId: address.CityId,
            AreaId: address.AreaId,
            Street: address.Street,
            BuildingNumber: address.BuildingNumber,
            Floor: address.Floor,
            Apartment: address.Apartment,
            Latitude: address.Latitude,
            Longitude: address.Longitude,
            FullAddress: address.FullAddress,
            IsDefault: address.IsDefault);
    }

    public static PaymentMethodResponse ToPaymentMethodResponse(
        this CustomerPaymentMethod paymentMethod)
    {
        ArgumentNullException.ThrowIfNull(paymentMethod);

        return new PaymentMethodResponse(
            PaymentMethodId: paymentMethod.Id,
            Type: paymentMethod.Type,
            IsDefault: paymentMethod.IsDefault);
    }
}
