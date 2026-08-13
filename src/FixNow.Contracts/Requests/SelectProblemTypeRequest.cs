using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record SelectProblemTypeRequest
{
    [Required(
        ErrorMessage = "Problem type is required.")]
    public Guid ProblemTypeId { get; init; }
}
