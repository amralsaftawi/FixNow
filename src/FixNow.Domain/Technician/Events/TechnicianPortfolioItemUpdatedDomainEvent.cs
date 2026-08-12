
public sealed record TechnicianPortfolioItemUpdatedDomainEvent(
    Guid TechnicianPortfolioItemId,
    Guid TechnicianProfileId)
    : DomainEvent;
