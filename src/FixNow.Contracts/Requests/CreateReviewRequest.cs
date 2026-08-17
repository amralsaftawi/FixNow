using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record CreateReviewRequest
{
    [Required(
        ErrorMessage = "Job id is required.")]
    public Guid JobId { get; init; }

    [Required(
        ErrorMessage = "Comment is required.")]
    [MaxLength(
        1000,
        ErrorMessage = "Comment cannot exceed 1000 characters.")]
    public string Comment { get; init; } = string.Empty;
}
