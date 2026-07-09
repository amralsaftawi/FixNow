
public sealed record ServiceCategoryDeactivatedDomainEvent(
    Guid ServiceCategoryId)
    : DomainEvent;