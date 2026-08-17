using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Commands.UpdateTechnicianLocation;

public sealed record UpdateTechnicianLocationCommand(
    Guid JobId,
    decimal Latitude,
    decimal Longitude)
    : ICommand<Result<Success>>;
