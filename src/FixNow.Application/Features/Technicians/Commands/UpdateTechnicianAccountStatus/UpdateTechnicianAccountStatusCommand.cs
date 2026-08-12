using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAccountStatus;

public sealed record UpdateTechnicianAccountStatusCommand(
    Guid TechnicianProfileId,
    AccountStatus Status)
    : ICommand<Result<TechnicianAccountStatusResponse>>;
