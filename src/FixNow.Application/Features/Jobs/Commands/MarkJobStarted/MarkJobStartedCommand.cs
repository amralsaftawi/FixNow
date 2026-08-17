using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Commands.MarkJobStarted;

public sealed record MarkJobStartedCommand(
    Guid JobId)
    : ICommand<Result<Success>>;
