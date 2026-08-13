using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAvailability;

public sealed record UpdateTechnicianAvailabilityCommand(
    TechnicianAvailability Availability)
    : ICommand<Result<TechnicianAvailabilityResponse>>;
