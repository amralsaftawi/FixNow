namespace FixNow.Contracts.Responses;

public sealed record TechnicianAvailabilityResponse(
    Guid TechnicianProfileId,
    TechnicianAvailability Availability);
