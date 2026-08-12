using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.VerifyTechnician;

public sealed record VerifyTechnicianCommand(
    Guid TechnicianProfileId)
    : ICommand<Result<TechnicianProfileResponse>>;
