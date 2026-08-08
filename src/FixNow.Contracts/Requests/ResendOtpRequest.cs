using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record ResendOtpRequest
{
    [Required(ErrorMessage = "Identifier is required.")]
    [StringLength(
        254,
        MinimumLength = 3,
        ErrorMessage = "Identifier must be between 3 and 254 characters.")]
    public string Identifier { get; init; } = string.Empty;

    [Required(ErrorMessage = "OTP purpose is required.")]
    [StringLength(
        50,
        MinimumLength = 3,
        ErrorMessage = "OTP purpose must be between 3 and 50 characters.")]
    public string Purpose { get; init; } = string.Empty;
}