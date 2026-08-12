using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record CreateTechnicianPortfolioItemRequest
{
    [Required(
        ErrorMessage = "Title is required.")]
    [MaxLength(
        150,
        ErrorMessage = "Title cannot exceed 150 characters.")]
    public string Title { get; init; } = string.Empty;

    [MaxLength(
        1000,
        ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; init; }

    public List<string>? MediaKeys { get; init; }
}
