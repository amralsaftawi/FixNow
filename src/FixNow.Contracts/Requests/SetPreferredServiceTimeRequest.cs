using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record SetPreferredServiceTimeRequest
{
    [Required(
        ErrorMessage = "Preferred service time is required.")]
    public DateTimeOffset? PreferredServiceTime { get; init; }
}
