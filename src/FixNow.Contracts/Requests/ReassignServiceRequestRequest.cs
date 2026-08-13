namespace FixNow.Contracts.Requests;

public sealed record ReassignServiceRequestRequest
{
    public Guid TechnicianProfileId { get; init; }
}
