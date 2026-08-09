using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record LoginRequest
{
    [Required(ErrorMessage = "Identifier is required.")]
    [MaxLength(256,ErrorMessage = "Identifier cannot exceed 256 characters.")]
    public string Identifier { get; init; } = string.Empty;

    [Required( ErrorMessage = "Password is required.")]
    [MaxLength( 100, ErrorMessage = "Password cannot exceed 100 characters.")]
    public string Password { get; init; } = string.Empty;
}