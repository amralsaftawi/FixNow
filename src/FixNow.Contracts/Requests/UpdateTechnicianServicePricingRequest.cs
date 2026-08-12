using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record UpdateTechnicianServicePricingRequest
{
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
