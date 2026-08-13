using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceRequests.Commands.AddProblemDescription;

public sealed record AddProblemDescriptionCommand(
    Guid ServiceRequestId,
    string Description)
    : ICommand<Result<Success>>;
