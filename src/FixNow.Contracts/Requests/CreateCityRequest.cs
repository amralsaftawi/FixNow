using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record CreateCityRequest
{
    [Required(
        ErrorMessage = "City name is required.")]
    [MaxLength(
        100,
        ErrorMessage = "City name cannot exceed 100 characters.")]
    public string Name { get; init; } = string.Empty;
}
