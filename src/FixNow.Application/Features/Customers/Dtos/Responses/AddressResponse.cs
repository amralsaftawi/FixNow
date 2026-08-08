namespace FixNow.Application.Features.CustomerProfiles.Dtos.Responses;

public sealed record AddressResponse(
    Guid AddressId,
    string Label,
    int CountryId,
    int CityId,
    int AreaId,
    string Street,
    string BuildingNumber,
    string? Floor,
    string? Apartment,
    decimal Latitude,
    decimal Longitude,
    string FullAddress,
    bool IsDefault);
