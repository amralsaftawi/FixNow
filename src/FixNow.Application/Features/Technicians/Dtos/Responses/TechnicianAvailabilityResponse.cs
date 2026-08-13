namespace FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

public sealed record TechnicianAvailabilityResponse(
    Guid TechnicianProfileId,
    TechnicianAvailability Availability);
