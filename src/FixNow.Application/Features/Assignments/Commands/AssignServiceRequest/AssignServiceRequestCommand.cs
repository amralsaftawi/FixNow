using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Assignments.Commands.AssignServiceRequest;

public sealed record AssignServiceRequestCommand(
    Guid ServiceRequestId,
    Guid TechnicianProfileId)
    : ICommand<Result<Success>>;
