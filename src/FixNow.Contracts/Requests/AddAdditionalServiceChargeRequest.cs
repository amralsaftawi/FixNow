using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record AddAdditionalServiceChargeRequest
{
    [Required(
        ErrorMessage = "Description is required.")]
    [MaxLength(
        500,
        ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; init; } = string.Empty;

    [Range(
        0.01,
        double.MaxValue,
        ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; init; }

    [EnumDataType(
        typeof(Currency),
        ErrorMessage = "Currency is invalid.")]
    public Currency Currency { get; init; }
}
