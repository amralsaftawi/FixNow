using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record RefreshTokenRequest
{
    [Required(ErrorMessage = "Refresh token is required.")]
    [MaxLength(500, ErrorMessage = "Refresh token cannot exceed 500 characters.")]
    public string RefreshToken { get; init; } = string.Empty;
}