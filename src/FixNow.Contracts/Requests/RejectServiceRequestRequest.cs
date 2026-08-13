namespace FixNow.Contracts.Requests;

public sealed record RejectServiceRequestRequest
{
    public AssignmentRejectReason Reason { get; init; }
}
