using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceRequests.Commands.SelectProblemType;

public sealed record SelectProblemTypeCommand(
    Guid ServiceRequestId,
    Guid ProblemTypeId)
    : ICommand<Result<Success>>;
