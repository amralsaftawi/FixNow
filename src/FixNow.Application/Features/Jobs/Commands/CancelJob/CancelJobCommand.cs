using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Commands.CancelJob;

public sealed record CancelJobCommand(
    Guid JobId)
    : ICommand<Result<Success>>;
