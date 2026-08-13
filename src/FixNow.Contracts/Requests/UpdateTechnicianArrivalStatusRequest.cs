namespace FixNow.Contracts.Requests;

public sealed record UpdateTechnicianArrivalStatusRequest
{
    public TechnicianArrivalStatus Status { get; init; }
}
