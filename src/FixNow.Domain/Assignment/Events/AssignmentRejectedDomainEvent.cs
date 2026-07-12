public sealed record AssignmentRejectedDomainEvent(
    Guid AssignmentId,
    Guid ServiceRequestId,
    Guid TechnicianProfileId,
    AssignmentRejectReason Reason)
    : DomainEvent;