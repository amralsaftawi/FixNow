using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianRequests.Commands.AcceptServiceRequest;

public sealed record AcceptServiceRequestCommand(
    Guid ServiceRequestId)
    : ICommand<Result<Success>>;
