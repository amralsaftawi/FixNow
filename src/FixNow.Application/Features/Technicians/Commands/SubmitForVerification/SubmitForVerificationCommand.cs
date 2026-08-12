using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.SubmitForVerification;

public sealed record SubmitForVerificationCommand
    : ICommand<Result<TechnicianProfileResponse>>;
