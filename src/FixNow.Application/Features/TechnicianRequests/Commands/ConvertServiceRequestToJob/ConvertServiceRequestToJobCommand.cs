using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianRequests.Commands.ConvertServiceRequestToJob;

public sealed record ConvertServiceRequestToJobCommand(
    Guid ServiceRequestId)
    : ICommand<Result<Success>>;
