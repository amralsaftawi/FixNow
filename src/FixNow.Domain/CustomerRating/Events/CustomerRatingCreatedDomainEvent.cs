public sealed record CustomerRatingCreatedDomainEvent(
    Guid CustomerRatingId,
    Guid CustomerProfileId) : DomainEvent;
