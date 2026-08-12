using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RejectTechnicianVerification;

public sealed record RejectTechnicianVerificationCommand(
    Guid TechnicianProfileId)
    : ICommand<Result<TechnicianProfileResponse>>;
