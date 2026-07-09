public sealed class Address : AuditableEntity
{
    public Guid CustomerProfileId { get; private set; }

    public string Label { get; private set; }

    public int CountryId { get; private set; }

    public int CityId { get; private set; }

    public int AreaId { get; private set; }

    public string Street { get; private set; }

    public string BuildingNumber { get; private set; }

    public string? Floor { get; private set; }

    public string? Apartment { get; private set; }

    public decimal Latitude { get; private set; }

    public decimal Longitude { get; private set; }

    public string FullAddress { get; private set; }

    public bool IsDefault { get; private set; }

    // Navigation

    public CustomerProfile CustomerProfile { get; private set; } = null!;

    private Address()
    {
    }

    private Address(
        Guid id,
        Guid customerProfileId,
        string label,
        int countryId,
        int cityId,
        int areaId,
        string street,
        string buildingNumber,
        string? floor,
        string? apartment,
        decimal latitude,
        decimal longitude,
        string fullAddress,
        bool isDefault)
        : base(id)
    {
        CustomerProfileId = customerProfileId;
        Label = label;
        CountryId = countryId;
        CityId = cityId;
        AreaId = areaId;
        Street = street;
        BuildingNumber = buildingNumber;
        Floor = floor;
        Apartment = apartment;
        Latitude = latitude;
        Longitude = longitude;
        FullAddress = fullAddress;
        IsDefault = isDefault;
    }

    public static Result<Address> Create(
        Guid id,
        Guid customerProfileId,
        string label,
        int countryId,
        int cityId,
        int areaId,
        string street,
        string buildingNumber,
        string? floor,
        string? apartment,
        decimal latitude,
        decimal longitude,
        string fullAddress,
        bool isDefault = false)
    {
        if (id == Guid.Empty)
            return AddressErrors.IdRequired;

        if (customerProfileId == Guid.Empty)
            return AddressErrors.CustomerProfileIdRequired;

        if (string.IsNullOrWhiteSpace(label))
            return AddressErrors.LabelRequired;

        if (countryId <= 0)
            return AddressErrors.CountryRequired;

        if (cityId <= 0)
            return AddressErrors.CityRequired;

        if (areaId <= 0)
            return AddressErrors.AreaRequired;

        if (string.IsNullOrWhiteSpace(street))
            return AddressErrors.StreetRequired;

        if (string.IsNullOrWhiteSpace(buildingNumber))
            return AddressErrors.BuildingNumberRequired;

        if (string.IsNullOrWhiteSpace(fullAddress))
            return AddressErrors.FullAddressRequired;

        var address = new Address(
            id,
            customerProfileId,
            label.Trim(),
            countryId,
            cityId,
            areaId,
            street.Trim(),
            buildingNumber.Trim(),
            floor?.Trim(),
            apartment?.Trim(),
            latitude,
            longitude,
            fullAddress.Trim(),
            isDefault);

        address.AddDomainEvent(
            new AddressCreatedDomainEvent(
                address.Id,
                address.CustomerProfileId));

        return address;
    }

    public Result<Success> Update(
        string label,
        int countryId,
        int cityId,
        int areaId,
        string street,
        string buildingNumber,
        string? floor,
        string? apartment,
        decimal latitude,
        decimal longitude,
        string fullAddress)
    {
        if (string.IsNullOrWhiteSpace(label))
            return AddressErrors.LabelRequired;

        if (countryId <= 0)
            return AddressErrors.CountryRequired;

        if (cityId <= 0)
            return AddressErrors.CityRequired;

        if (areaId <= 0)
            return AddressErrors.AreaRequired;

        if (string.IsNullOrWhiteSpace(street))
            return AddressErrors.StreetRequired;

        if (string.IsNullOrWhiteSpace(buildingNumber))
            return AddressErrors.BuildingNumberRequired;

        if (string.IsNullOrWhiteSpace(fullAddress))
            return AddressErrors.FullAddressRequired;

        Label = label.Trim();
        CountryId = countryId;
        CityId = cityId;
        AreaId = areaId;
        Street = street.Trim();
        BuildingNumber = buildingNumber.Trim();
        Floor = floor?.Trim();
        Apartment = apartment?.Trim();
        Latitude = latitude;
        Longitude = longitude;
        FullAddress = fullAddress.Trim();

        AddDomainEvent(
            new AddressUpdatedDomainEvent(
                Id,
                CustomerProfileId));

        return Result.Success;
    }

    public Result<Success> SetAsDefault()
    {
        if (IsDefault)
            return AddressErrors.AlreadyDefault;

        IsDefault = true;

        AddDomainEvent(
            new AddressMarkedAsDefaultDomainEvent(
                Id,
                CustomerProfileId));

        return Result.Success;
    }

    public Result<Success> RemoveDefault()
    {
        if (!IsDefault)
            return Result.Success;

        IsDefault = false;

        return Result.Success;
    }
}