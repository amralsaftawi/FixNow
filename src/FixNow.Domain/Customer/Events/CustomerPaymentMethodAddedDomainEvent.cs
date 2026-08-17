
public sealed record CustomerPaymentMethodAddedDomainEvent(
    Guid CustomerProfileId,
    Guid PaymentMethodId) : DomainEvent;
