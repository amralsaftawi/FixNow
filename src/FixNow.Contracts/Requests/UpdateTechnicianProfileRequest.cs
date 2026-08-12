using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record UpdateTechnicianProfileRequest
{
    [Range(0, int.MaxValue, ErrorMessage = "Years of experience cannot be negative.")]
    public int YearsOfExperience { get; init; }

    [MaxLength(1000, ErrorMessage = "Bio cannot exceed 1000 characters.")]
    public string? Bio { get; init; }

    [MaxLength(500, ErrorMessage = "National ID image key cannot exceed 500 characters.")]
    public string? NationalIdImageKey { get; init; }
}
