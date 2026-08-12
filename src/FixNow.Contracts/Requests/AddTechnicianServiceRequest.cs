using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record AddTechnicianServiceRequest
{
    [Required(
        ErrorMessage = "Service category id is required.")]
    public Guid ServiceCategoryId { get; init; }
}
