
public sealed record ServiceRequestCategoryChangedDomainEvent(
    Guid ServiceRequestId,
    Guid ServiceCategoryId) : DomainEvent;
