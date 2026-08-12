using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record UpdateTechnicianExperienceRequest
{
    [Required(
        ErrorMessage = "Company name is required.")]
    [MaxLength(
        150,
        ErrorMessage = "Company name cannot exceed 150 characters.")]
    public string CompanyName { get; init; } = string.Empty;

    [Required(
        ErrorMessage = "Position is required.")]
    [MaxLength(
        150,
        ErrorMessage = "Position cannot exceed 150 characters.")]
    public string Position { get; init; } = string.Empty;

    [MaxLength(
        1000,
        ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; init; }

    public DateTimeOffset StartDate { get; init; }

    public DateTimeOffset? EndDate { get; init; }
}
