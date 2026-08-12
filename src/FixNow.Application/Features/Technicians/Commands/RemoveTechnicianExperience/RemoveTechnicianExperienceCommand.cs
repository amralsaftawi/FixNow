using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RemoveTechnicianExperience;

public sealed record RemoveTechnicianExperienceCommand(
    Guid ExperienceId)
    : ICommand<Result<Success>>;
