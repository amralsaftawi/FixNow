using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianRequests.Commands.RejectServiceRequest;

public sealed record RejectServiceRequestCommand(
    Guid ServiceRequestId,
    AssignmentRejectReason Reason)
    : ICommand<Result<Success>>;
