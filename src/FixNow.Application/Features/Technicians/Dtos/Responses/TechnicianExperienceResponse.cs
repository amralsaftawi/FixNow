namespace FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

public sealed record TechnicianExperienceResponse(
    Guid TechnicianExperienceId,
    Guid TechnicianProfileId,
    string CompanyName,
    string Position,
    string? Description,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate,
    bool IsCurrent);
