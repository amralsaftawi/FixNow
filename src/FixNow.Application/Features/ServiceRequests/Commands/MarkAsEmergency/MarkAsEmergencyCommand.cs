using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceRequests.Commands.MarkAsEmergency;

public sealed record MarkAsEmergencyCommand(
    Guid ServiceRequestId)
    : ICommand<Result<Success>>;
