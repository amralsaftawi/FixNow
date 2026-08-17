using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record RateCustomerRequest
{
    [Range(
        1,
        5,
        ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; init; }

    [MaxLength(1000)]
    public string? Comment { get; init; }
}
