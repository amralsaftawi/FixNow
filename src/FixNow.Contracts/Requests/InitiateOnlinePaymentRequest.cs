using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record InitiateOnlinePaymentRequest
{
    [Required(
        ErrorMessage = "Job id is required.")]
    public Guid JobId { get; init; }
}
