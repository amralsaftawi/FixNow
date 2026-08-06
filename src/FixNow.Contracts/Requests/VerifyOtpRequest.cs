using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record VerifyOtpRequest
{
    [Required(ErrorMessage = "Identifier is required.")]
    [MaxLength(256, ErrorMessage = "Identifier cannot exceed 256 characters.")]
    public string Identifier { get; init; } = string.Empty;

    [Required(ErrorMessage = "Otp is required.")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Otp must be exactly 6 digits.")]
    public string Otp { get; init; } = string.Empty;

    [Required(ErrorMessage = "Purpose is required.")]
    public string Purpose { get; init; } = string.Empty;
}
