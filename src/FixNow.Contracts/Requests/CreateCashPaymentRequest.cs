using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record CreateCashPaymentRequest
{
    [Required(
        ErrorMessage = "Job id is required.")]
    public Guid JobId { get; init; }
}
