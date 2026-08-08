using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record CreateAreaRequest
{
    [Required(
        ErrorMessage = "Area name is required.")]
    [MaxLength(
        100,
        ErrorMessage = "Area name cannot exceed 100 characters.")]
    public string Name { get; init; } = string.Empty;
}
