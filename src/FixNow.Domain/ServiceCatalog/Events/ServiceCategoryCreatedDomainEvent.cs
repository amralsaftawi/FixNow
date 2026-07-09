

public sealed record ServiceCategoryCreatedDomainEvent(
    Guid ServiceCategoryId)
    : DomainEvent;