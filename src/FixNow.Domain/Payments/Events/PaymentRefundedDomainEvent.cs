public sealed record PaymentRefundedDomainEvent(
    Guid PaymentId,
    Guid AssignmentId,
    Guid CustomerProfileId,
    decimal Amount)
    : DomainEvent;