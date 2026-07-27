using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.CompleteTechnicianProfile;

public sealed record CompleteTechnicianProfileCommand
    : ICommand<Result<Updated>>;