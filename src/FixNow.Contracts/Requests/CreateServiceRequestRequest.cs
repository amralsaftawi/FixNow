using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record CreateServiceRequestRequest
{
    [Required(
        ErrorMessage = "Address is required.")]
    public Guid AddressId { get; init; }

    [Required(
        ErrorMessage = "Service category is required.")]
    public Guid ServiceCategoryId { get; init; }

    [Required(
        ErrorMessage = "Description is required.")]
    [MaxLength(
        2000,
        ErrorMessage = "Description cannot exceed 2000 characters.")]
    public string Description { get; init; } = string.Empty;

    public ServicePriority Priority { get; init; }

    public DateTimeOffset? ScheduledAt { get; init; }
}
