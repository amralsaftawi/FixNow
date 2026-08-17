using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Commands.UpdateJobStatus;

public sealed record UpdateJobStatusCommand(
    Guid JobId,
    JobStatus Status)
    : ICommand<Result<Success>>;
