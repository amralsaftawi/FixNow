using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianProfile;

public sealed record UpdateTechnicianProfileCommand(
    int YearsOfExperience,
    string? Bio,
    string? NationalIdImageKey)
    : ICommand<Result<Updated>>;