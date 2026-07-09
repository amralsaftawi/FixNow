
public sealed record ServiceCategoryActivatedDomainEvent(
    Guid ServiceCategoryId)
    : DomainEvent;