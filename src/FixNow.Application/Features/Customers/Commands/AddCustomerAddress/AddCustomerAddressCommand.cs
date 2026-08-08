using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.CustomerProfiles.Commands.AddCustomerAddress;

public sealed record AddCustomerAddressCommand(
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
    string FullAddress)
    : ICommand<Result<Created>>;
