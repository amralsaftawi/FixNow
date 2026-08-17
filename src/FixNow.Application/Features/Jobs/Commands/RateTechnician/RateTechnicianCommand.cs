using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Commands.RateTechnician;

public sealed record RateTechnicianCommand(
    Guid JobId,
    int Rating,
    string? Comment)
    : ICommand<Result<RateTechnicianResponse>>;
