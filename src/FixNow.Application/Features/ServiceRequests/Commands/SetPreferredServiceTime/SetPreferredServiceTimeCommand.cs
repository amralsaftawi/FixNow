using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceRequests.Commands.SetPreferredServiceTime;

public sealed record SetPreferredServiceTimeCommand(
    Guid ServiceRequestId,
    DateTimeOffset PreferredServiceTime)
    : ICommand<Result<Success>>;
