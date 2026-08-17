
public sealed record CustomerPaymentMethodMarkedAsDefaultDomainEvent(
    Guid PaymentMethodId,
    Guid CustomerProfileId) : DomainEvent;
