using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Commands.ConfirmServiceCompletion;

public sealed record ConfirmServiceCompletionCommand(
    Guid JobId)
    : ICommand<Result<Success>>;
