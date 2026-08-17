
public sealed record JobAdditionalChargeAddedDomainEvent(
    Guid JobId,
    Guid AdditionalChargeId,
    string Description,
    Money Amount) : DomainEvent;
