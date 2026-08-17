
public sealed record CustomerPaymentMethodCreatedDomainEvent(
    Guid PaymentMethodId,
    Guid CustomerProfileId) : DomainEvent;
