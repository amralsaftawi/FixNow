using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceRequests.Commands.CancelServiceRequest;

public sealed record CancelServiceRequestCommand(
    Guid ServiceRequestId)
    : ICommand<Result<Success>>;
