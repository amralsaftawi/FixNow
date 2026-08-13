using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceRequests.Commands.CompleteServiceRequest;

public sealed record CompleteServiceRequestCommand(
    Guid ServiceRequestId)
    : ICommand<Result<Success>>;
