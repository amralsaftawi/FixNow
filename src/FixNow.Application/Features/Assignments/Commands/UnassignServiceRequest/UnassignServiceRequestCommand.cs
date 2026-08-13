using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Assignments.Commands.UnassignServiceRequest;

public sealed record UnassignServiceRequestCommand(
    Guid ServiceRequestId)
    : ICommand<Result<Success>>;
