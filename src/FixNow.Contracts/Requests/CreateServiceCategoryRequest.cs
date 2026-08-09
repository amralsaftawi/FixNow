using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record CreateServiceCategoryRequest
{
    [Required(
        ErrorMessage = "Service category name is required.")]
    [MaxLength(
        100,
        ErrorMessage = "Service category name cannot exceed 100 characters.")]
    public string Name { get; init; } = string.Empty;

    [Required(
        ErrorMessage = "Service category description is required.")]
    [MaxLength(
        500,
        ErrorMessage = "Service category description cannot exceed 500 characters.")]
    public string Description { get; init; } = string.Empty;

    [Required(
        ErrorMessage = "Service category icon is required.")]
    [MaxLength(
        255,
        ErrorMessage = "Service category icon cannot exceed 255 characters.")]
    public string IconKey { get; init; } = string.Empty;

    [Range(
        0,
        int.MaxValue,
        ErrorMessage = "Display order must be greater than or equal to zero.")]
    public int DisplayOrder { get; init; }
}
