using FluentValidation;

namespace FixNow.Application.Features.CustomerProfiles.Commands.UpdateCustomerAddress;

public sealed class UpdateCustomerAddressCommandValidator
    : AbstractValidator<UpdateCustomerAddressCommand>
{
    public UpdateCustomerAddressCommandValidator()
    {
        ValidateAddressId();

        ValidateLabel();

        ValidateCountryId();

        ValidateCityId();

        ValidateAreaId();

        ValidateStreet();

        ValidateBuildingNumber();

        ValidateFloor();

        ValidateApartment();

        ValidateFullAddress();

        ValidateLatitude();

        ValidateLongitude();
    }

    private void ValidateAddressId()
    {
        RuleFor(x => x.AddressId)
            .NotEmpty()
            .WithErrorCode("Address.IdRequired");
    }

    private void ValidateLabel()
    {
        RuleFor(x => x.Label)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Address.LabelRequired")
            .MaximumLength(100)
            .WithErrorCode("Address.LabelTooLong");
    }

    private void ValidateCountryId()
    {
        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithErrorCode("Address.CountryRequired");
    }

    private void ValidateCityId()
    {
        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .WithErrorCode("Address.CityRequired");
    }

    private void ValidateAreaId()
    {
        RuleFor(x => x.AreaId)
            .GreaterThan(0)
            .WithErrorCode("Address.AreaRequired");
    }

    private void ValidateStreet()
    {
        RuleFor(x => x.Street)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Address.StreetRequired")
            .MaximumLength(200)
            .WithErrorCode("Address.StreetTooLong");
    }

    private void ValidateBuildingNumber()
    {
        RuleFor(x => x.BuildingNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Address.BuildingNumberRequired")
            .MaximumLength(50)
            .WithErrorCode("Address.BuildingNumberTooLong");
    }

    private void ValidateFloor()
    {
        RuleFor(x => x.Floor)
            .MaximumLength(50)
            .WithErrorCode("Address.FloorTooLong");
    }

    private void ValidateApartment()
    {
        RuleFor(x => x.Apartment)
            .MaximumLength(50)
            .WithErrorCode("Address.ApartmentTooLong");
    }

    private void ValidateFullAddress()
    {
        RuleFor(x => x.FullAddress)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Address.FullAddressRequired")
            .MaximumLength(500)
            .WithErrorCode("Address.FullAddressTooLong");
    }

    private void ValidateLatitude()
    {
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .WithErrorCode("Address.LatitudeInvalid");
    }

    private void ValidateLongitude()
    {
        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .WithErrorCode("Address.LongitudeInvalid");
    }
}
