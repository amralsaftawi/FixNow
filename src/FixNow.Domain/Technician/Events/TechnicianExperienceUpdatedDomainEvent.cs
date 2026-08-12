
public sealed record TechnicianExperienceUpdatedDomainEvent(
    Guid TechnicianExperienceId,
    Guid TechnicianProfileId)
    : DomainEvent;
