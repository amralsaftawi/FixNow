using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record UpdateCurrentCustomerLocationRequest
{
    [Range(
        -90,
        90,
        ErrorMessage = "Latitude must be between -90 and 90.")]
    public decimal Latitude { get; init; }

    [Range(
        -180,
        180,
        ErrorMessage = "Longitude must be between -180 and 180.")]
    public decimal Longitude { get; init; }
}
