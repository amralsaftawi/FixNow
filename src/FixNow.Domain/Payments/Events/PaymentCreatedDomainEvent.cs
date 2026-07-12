public sealed record PaymentCreatedDomainEvent(
    Guid PaymentId,
    Guid AssignmentId,
    Guid CustomerProfileId,
    decimal Amount,
    PaymentMethod PaymentMethod)
    : DomainEvent;