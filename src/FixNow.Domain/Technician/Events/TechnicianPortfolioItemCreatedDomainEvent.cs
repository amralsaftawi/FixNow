
public sealed record TechnicianPortfolioItemCreatedDomainEvent(
    Guid TechnicianPortfolioItemId,
    Guid TechnicianProfileId)
    : DomainEvent;
