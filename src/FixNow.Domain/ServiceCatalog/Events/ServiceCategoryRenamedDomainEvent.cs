

public sealed record ServiceCategoryRenamedDomainEvent(
    Guid ServiceCategoryId)
    : DomainEvent;