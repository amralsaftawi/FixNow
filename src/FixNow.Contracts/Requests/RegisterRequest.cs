using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record RegisterRequest
{
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(100,ErrorMessage = "First name cannot exceed 100 characters.")]
    public string FirstName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(100,ErrorMessage = "Last name cannot exceed 100 characters.")]
    public string LastName { get; init; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [MaxLength(320,ErrorMessage = "Email cannot exceed 256 characters.")]
    public string? Email { get; init; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8,ErrorMessage = "Password must be at least 8 characters.")]
    [MaxLength(100,ErrorMessage = "Password cannot exceed 100 characters.")]
    public string Password { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password confirmation is required.")]
    [Compare(nameof(Password),ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; init; } = string.Empty;

  [Required]
  [StringLength(2, MinimumLength = 2, ErrorMessage = "CountryCode must be exactly 2 characters.")]
public string CountryCode { get; init; } = string.Empty;

   [EnumDataType(typeof(PreferredLanguage))]
public PreferredLanguage PreferredLanguage { get; init; }
}
