public sealed record PaymentSucceededDomainEvent(
    Guid PaymentId,
    Guid AssignmentId,
    Guid CustomerProfileId,
    decimal Amount,
    DateTimeOffset PaidAt)
    : DomainEvent;