using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record AddProblemDescriptionRequest
{
    [Required(
        ErrorMessage = "Problem description is required.")]
    [MaxLength(
        2000,
        ErrorMessage = "Problem description cannot exceed 2000 characters.")]
    public string Description { get; init; } = string.Empty;
}
