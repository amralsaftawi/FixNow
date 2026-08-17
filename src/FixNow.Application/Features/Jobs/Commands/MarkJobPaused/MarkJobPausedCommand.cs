using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Commands.MarkJobPaused;

public sealed record MarkJobPausedCommand(
    Guid JobId)
    : ICommand<Result<Success>>;
