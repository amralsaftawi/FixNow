
public sealed record ServiceRequestProblemTypeChangedDomainEvent(
    Guid ServiceRequestId,
    Guid ProblemTypeId) : DomainEvent;
