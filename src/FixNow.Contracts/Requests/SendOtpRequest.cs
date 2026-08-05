using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record SendOtpRequest
{
    [Required(ErrorMessage = "Identifier is required.")]
    [MaxLength(320, ErrorMessage = "Identifier cannot exceed 320 characters.")]
    public string Identifier { get; init; } = string.Empty;
}
