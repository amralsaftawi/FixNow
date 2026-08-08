using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record UpdateCustomerAddressRequest
{
    [Required(
        ErrorMessage = "Label is required.")]
    [MaxLength(
        100,
        ErrorMessage = "Label cannot exceed 100 characters.")]
    public string Label { get; init; } = string.Empty;

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "Country is required.")]
    public int CountryId { get; init; }

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "City is required.")]
    public int CityId { get; init; }

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "Area is required.")]
    public int AreaId { get; init; }

    [Required(
        ErrorMessage = "Street is required.")]
    [MaxLength(
        200,
        ErrorMessage = "Street cannot exceed 200 characters.")]
    public string Street { get; init; } = string.Empty;

    [Required(
        ErrorMessage = "Building number is required.")]
    [MaxLength(
        50,
        ErrorMessage = "Building number cannot exceed 50 characters.")]
    public string BuildingNumber { get; init; } = string.Empty;

    [MaxLength(
        50,
        ErrorMessage = "Floor cannot exceed 50 characters.")]
    public string? Floor { get; init; }

    [MaxLength(
        50,
        ErrorMessage = "Apartment cannot exceed 50 characters.")]
    public string? Apartment { get; init; }

    public decimal Latitude { get; init; }

    public decimal Longitude { get; init; }

    [Required(
        ErrorMessage = "Full address is required.")]
    [MaxLength(
        500,
        ErrorMessage = "Full address cannot exceed 500 characters.")]
    public string FullAddress { get; init; } = string.Empty;
}
