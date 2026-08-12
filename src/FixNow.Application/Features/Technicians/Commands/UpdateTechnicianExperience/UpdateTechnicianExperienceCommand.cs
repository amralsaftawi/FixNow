using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianExperience;

public sealed record UpdateTechnicianExperienceCommand(
    Guid ExperienceId,
    string CompanyName,
    string Position,
    string? Description,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate)
    : ICommand<Result<TechnicianExperienceResponse>>;
