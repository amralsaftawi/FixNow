using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record UpdateCurrentUserRequest
{
    [Required(
        ErrorMessage = "First name is required.")]
    [MaxLength(
        100,
        ErrorMessage = "First name cannot exceed 100 characters.")]
    public string FirstName { get; init; } = string.Empty;

    [Required(
        ErrorMessage = "Last name is required.")]
    [MaxLength(
        100,
        ErrorMessage = "Last name cannot exceed 100 characters.")]
    public string LastName { get; init; } = string.Empty;

    [EmailAddress(
        ErrorMessage = "Invalid email address.")]
    [MaxLength(
        320,
        ErrorMessage = "Email cannot exceed 320 characters.")]
    public string? Email { get; init; }

    [Required(
        ErrorMessage = "Phone number is required.")]
    [MaxLength(
        20,
        ErrorMessage = "Phone number cannot exceed 20 characters.")]
    public string PhoneNumber { get; init; } = string.Empty;

    [Required(
        ErrorMessage = "Country code is required.")]
    [StringLength(
        2,
        MinimumLength = 2,
        ErrorMessage = "Country code must be exactly 2 characters.")]
    public string CountryCode { get; init; } = string.Empty;

    [EnumDataType(
        typeof(PreferredLanguage),
        ErrorMessage = "Preferred language is invalid.")]
    public PreferredLanguage PreferredLanguage { get; init; }
}
