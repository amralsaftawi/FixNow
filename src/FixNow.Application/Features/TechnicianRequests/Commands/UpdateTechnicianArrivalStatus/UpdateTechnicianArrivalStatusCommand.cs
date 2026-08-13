using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianRequests.Commands.UpdateTechnicianArrivalStatus;

public sealed record UpdateTechnicianArrivalStatusCommand(
    Guid ServiceRequestId,
    TechnicianArrivalStatus Status)
    : ICommand<Result<Success>>;
