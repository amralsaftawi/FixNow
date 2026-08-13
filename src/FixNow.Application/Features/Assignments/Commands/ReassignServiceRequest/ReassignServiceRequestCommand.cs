using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Assignments.Commands.ReassignServiceRequest;

public sealed record ReassignServiceRequestCommand(
    Guid ServiceRequestId,
    Guid TechnicianProfileId)
    : ICommand<Result<Success>>;
