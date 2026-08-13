namespace FixNow.Contracts.Requests;

public sealed record AssignServiceRequestRequest
{
    public Guid ServiceRequestId { get; init; }

    public Guid TechnicianProfileId { get; init; }
}
