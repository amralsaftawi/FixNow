
public sealed record CustomerPaymentMethodRemovedDomainEvent(
    Guid CustomerProfileId,
    Guid PaymentMethodId) : DomainEvent;
