using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceRequests.Commands.MarkSearching;

public sealed record MarkSearchingCommand(
    Guid ServiceRequestId)
    : ICommand<Result<Success>>;
