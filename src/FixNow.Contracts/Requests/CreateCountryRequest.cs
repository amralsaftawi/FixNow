using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record CreateCountryRequest
{
    [Required(
        ErrorMessage = "Country name is required.")]
    [MaxLength(
        100,
        ErrorMessage = "Country name cannot exceed 100 characters.")]
    public string Name { get; init; } = string.Empty;
}
