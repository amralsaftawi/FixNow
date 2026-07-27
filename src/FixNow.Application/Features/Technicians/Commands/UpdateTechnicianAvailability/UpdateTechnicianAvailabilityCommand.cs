using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAvailability;

public sealed record UpdateTechnicianAvailabilityCommand(
    TechnicianAvailability Availability)
    : ICommand<Result<Updated>>;