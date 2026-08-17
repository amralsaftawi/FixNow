using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Commands.MarkJobEnRoute;

public sealed record MarkJobEnRouteCommand(
    Guid JobId)
    : ICommand<Result<Success>>;
