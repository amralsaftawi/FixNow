
public sealed record CustomerDefaultPaymentMethodChangedDomainEvent(
    Guid CustomerProfileId,
    Guid PaymentMethodId) : DomainEvent;
