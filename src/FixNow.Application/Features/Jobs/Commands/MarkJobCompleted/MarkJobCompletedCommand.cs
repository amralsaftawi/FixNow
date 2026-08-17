using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Commands.MarkJobCompleted;

public sealed record MarkJobCompletedCommand(
    Guid JobId)
    : ICommand<Result<Success>>;
