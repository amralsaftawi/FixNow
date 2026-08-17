namespace FixNow.Contracts.Requests;

public sealed record UpdateJobStatusRequest
{
    public JobStatus Status { get; init; }
}
