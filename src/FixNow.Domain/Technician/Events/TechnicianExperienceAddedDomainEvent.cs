
public sealed record TechnicianExperienceAddedDomainEvent(
    Guid TechnicianExperienceId,
    Guid TechnicianProfileId)
    : DomainEvent;
