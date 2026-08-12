
public sealed record TechnicianExperienceRemovedDomainEvent(
    Guid TechnicianExperienceId,
    Guid TechnicianProfileId)
    : DomainEvent;
