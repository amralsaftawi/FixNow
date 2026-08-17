using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Commands.MarkJobArrived;

public sealed record MarkJobArrivedCommand(
    Guid JobId)
    : ICommand<Result<Success>>;
