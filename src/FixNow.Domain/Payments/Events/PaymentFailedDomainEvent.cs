public sealed record PaymentFailedDomainEvent(
    Guid PaymentId,
    Guid AssignmentId,
    Guid CustomerProfileId)
    : DomainEvent;