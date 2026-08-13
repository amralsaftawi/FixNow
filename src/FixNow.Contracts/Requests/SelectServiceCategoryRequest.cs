using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record SelectServiceCategoryRequest
{
    [Required(
        ErrorMessage = "Service category is required.")]
    public Guid ServiceCategoryId { get; init; }
}
