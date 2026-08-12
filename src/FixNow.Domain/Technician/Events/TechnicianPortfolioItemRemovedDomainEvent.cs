
public sealed record TechnicianPortfolioItemRemovedDomainEvent(
    Guid TechnicianPortfolioItemId,
    Guid TechnicianProfileId)
    : DomainEvent;
