namespace FixNow.Contracts.Requests;

public sealed record UpdateTechnicianAvailabilityRequest
{
    public TechnicianAvailability Availability { get; init; }
}
