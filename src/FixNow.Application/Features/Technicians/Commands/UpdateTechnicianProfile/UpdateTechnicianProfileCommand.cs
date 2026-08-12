using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianProfile;

public sealed record UpdateTechnicianProfileCommand(
    int YearsOfExperience,
    string? Bio,
    string? NationalIdImageKey)
    : ICommand<Result<TechnicianProfileResponse>>;
