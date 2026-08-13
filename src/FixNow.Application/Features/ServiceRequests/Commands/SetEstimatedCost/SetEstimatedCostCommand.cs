using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceRequests.Commands.SetEstimatedCost;

public sealed record SetEstimatedCostCommand(
    Guid ServiceRequestId,
    decimal Amount,
    Currency Currency)
    : ICommand<Result<Success>>;
